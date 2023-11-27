// Watch video here: https://www.youtube.com/watch?v=y4b-FcmOGV0

// Output for viewing with Serial Oscilloscope: accX,accY,magZ // gyrX, gyrY and gyrZ are commented out

/*
Arduino     MARG GY-85
  A5            SCL
  A4            SDA
  3.3V          VCC
  GND           GND
*/ 

#include <Wire.h>
#include <ADXL345.h>  // ADXL345 Accelerometer Library
#include <HMC5883L.h> // HMC5883L Magnetometer Library
#include <ITG3200.h>

ADXL345 acc; //variable adxl is an instance of the ADXL345 library
HMC5883L compass;
ITG3200 gyro = ITG3200();
float  gx,gy,gz;
//float  gx_rate, gy_rate, gz_rate;
//int ix, iy, iz;
//float anglegx=0.0, anglegy=0.0, anglegz=0.0;
int ax,ay,az;  
int rawX, rawY, rawZ;
float X, Y, Z;
int rolldeg, pitchdeg;
int error = 0; 
float aoffsetX, aoffsetY, aoffsetZ;
float goffsetX, goffsetY, goffsetZ;
unsigned long time, looptime;

void setup(){
  Serial.begin(9600);
  acc.powerOn();
  compass = HMC5883L();
  error = compass.SetScale(1.3); // Set the scale to +/- 1.3 Ga of the compass
  if(error != 0) // If there is an error, print it out.
    Serial.println(compass.GetErrorText(error));
  
 // Serial.println("Setting measurement mode to continous");
  error = compass.SetMeasurementMode(Measurement_Continuous); // Set the measurement mode to Continuous
  if(error != 0) // If there is an error, print it out.
    Serial.println(compass.GetErrorText(error));
    for (int i = 0; i <= 200; i++) {
    acc.readAccel(&ax, &ay, &az);
    if (i == 0) {
      aoffsetX = ax;
      aoffsetY = ay;
      aoffsetZ = az;
    }
    if (i > 1) {
      aoffsetX = (ax + aoffsetX) / 2;
      aoffsetY = (ay + aoffsetY) / 2;
      aoffsetZ = (az + aoffsetZ) / 2;
    }
  }
  for (int i = 0; i <= 200; i++) {
    gyro.readGyro(&gx,&gy,&gz); 
    if (i == 0) {
      goffsetX = gx;
      goffsetY = gy;
      goffsetZ = gz;
    }
    if (i > 1) {
      goffsetX = (gx + goffsetX) / 2;
      goffsetY = (gy + goffsetY) / 2;
      goffsetZ = (gz + goffsetZ) / 2;
    }
  }
  
  
  delay(1000);
  gyro.init(ITG3200_ADDR_AD0_LOW); 
 // Serial.print("zero Calibrating...");
 // gyro.zeroCalibrate(2500, 2);
 // Serial.println("done.");
}

void loop(){
  // code fragment for Accelerometer angles (roll and pitch)
  time = millis();
  acc.readAccel(&ax, &ay, &az); //read the accelerometer values and store them in variables  x,y,z
  rawX = ax - aoffsetX;
  rawY = ay - aoffsetY;
  rawZ = az  - (255 - aoffsetZ);
  X = rawX/256.00; // used for angle calculations
  Y = rawY/256.00; // used for angle calculations
  Z = rawZ/256.00; // used for angle calculations
  rolldeg = 180*(atan(Y/sqrt(X*X+Z*Z)))/PI; // calculated angle in degrees
  pitchdeg = 180*(atan(X/sqrt(Y*Y+Z*Z)))/PI * (-1); // calculated angle in degrees

// Accelerometer, Magnetometer and Gyroscope Output
  Serial.print(rolldeg);
  Serial.print(",");
  Serial.println(pitchdeg);  // calculated angle in degrees
  delay(100);
}


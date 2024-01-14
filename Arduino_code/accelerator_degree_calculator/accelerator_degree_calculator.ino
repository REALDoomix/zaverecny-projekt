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

ADXL345 acc; // variable adxl is an instance of the ADXL345 library
int ax, ay, az;
int rawX, rawY, rawZ;
float X, Y, Z;
int rolldeg, pitchdeg;
int error = 0;
float aoffsetX, aoffsetY, aoffsetZ;
unsigned long time, looptime;

void setup()
{
  Serial.begin(9600);
  acc.powerOn();
  
  for (int i = 0; i <= 200; i++)
  {
    acc.readAccel(&ax, &ay, &az);
    if (i == 0)
    {
      aoffsetX = ax;
      aoffsetY = ay;
      aoffsetZ = az;
    }
    if (i > 1)
    {
      aoffsetX = (ax + aoffsetX) / 2;
      aoffsetY = (ay + aoffsetY) / 2;
      aoffsetZ = (az + aoffsetZ) / 2;
    }
  }

  delay(1000);
}

void loop()
{
  // code fragment for Accelerometer angles (roll and pitch)
  time = millis();
  acc.readAccel(&ax, &ay, &az); // read the accelerometer values and store them in variables  x,y,z
  rawX = ax - aoffsetX;
  rawY = ay - aoffsetY;
  rawZ = az - (255 - aoffsetZ);
  X = rawX / 256.00;                                            // used for angle calculations
  Y = rawY / 256.00;                                            // used for angle calculations
  Z = rawZ / 256.00;                                            // used for angle calculations
  rolldeg = 180 * (atan(Y / sqrt(X * X + Z * Z))) / PI;         // calculated angle in degrees
  pitchdeg = 180 * (atan(X / sqrt(Y * Y + Z * Z))) / PI * (-1); // calculated angle in degrees

  // Accelerometer Output
  Serial.print(rolldeg);
  Serial.print(",");
  Serial.println(pitchdeg); // calculated angle in degrees
  delay(100);
}

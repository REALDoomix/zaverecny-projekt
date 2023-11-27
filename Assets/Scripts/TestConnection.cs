using System.Collections;
using UnityEngine;
using System.IO.Ports;


public class TestConnection : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("COM5", 9600);
    public string receivedString;
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.05f;

    public string[] datas;

    void Start()
    {
        data_stream.Open(); //Initiate the Serial stream
    }

    // Update is called once per frame
    void Update()
    {
        receivedString = data_stream.ReadLine();

        string[] datas = receivedString.Split(','); //split the data between ','
        rb.AddForce(0, 0, float.Parse(datas[1]) * sensitivity * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddForce(float.Parse(datas[0]) * sensitivity * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }
}
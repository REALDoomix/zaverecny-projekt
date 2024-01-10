using System.Collections;
using UnityEngine;
using System.IO.Ports;
using System;


public class TestConnection : MonoBehaviour
{
    private SerialPort data_stream;
    private string[] ports;
    private string portName;
    private bool portFound = false;
    
    public string receivedString;
    public GameObject test_data;
    public Rigidbody rb;
    public float sensitivity = 0.5f;

    public string[] datas;

    void Start()
    {
        ports = SerialPort.GetPortNames();
    for (int i = 0; i < ports.Length; i++)
        {
            portName = ports[i];
            data_stream = new SerialPort(portName, 9600);
            Debug.Log(portName);

            try
            {
                data_stream.Open();
                portFound = true;
                break;
            }
            catch (Exception e)
            {
                Debug.Log("Failed to open port " + portName + ": " + e.Message);
            }
        }

        if (!portFound)
        {
            Debug.LogError("Failed to find a valid COM port.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (data_stream != null && data_stream.IsOpen)
        {
        receivedString = data_stream.ReadLine();
        

        string[] datas = receivedString.Split(','); //split the data between ','
        rb.AddForce(0, 0, float.Parse(datas[1]) * sensitivity * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddForce(float.Parse(datas[0]) * sensitivity * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }
}
}
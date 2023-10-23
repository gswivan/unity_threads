
using System;
using UnityEngine;
using System.Threading;
using System.IO.Ports;
using UnityEngine.Events;
using System.Collections.Generic;

public class Arduino : MonoBehaviour
{
    public string PortName = "COM1";
    public UnityEvent PinPressed;
    public UnityEvent PinReleased;

    private Thread mThread;
    private bool mRunning;
    private Queue<string> mReadQueue;

    private void OnEnable()
    {
        mRunning = true;
        mReadQueue = new Queue<string>();
        mThread = new Thread(Run);
        mThread.Start();
    }

    private void OnDisable()
    {
        mRunning = false;
        mThread.Join();
    }

    private void Run()
    {
        Debug.Log("Starting Thread");
        SerialPort arduino = new SerialPort();
        arduino.PortName = PortName;
        arduino.BaudRate = 9600;
        arduino.Parity = Parity.None;
        arduino.DataBits = 8;
        arduino.StopBits = StopBits.One;
        arduino.Handshake = Handshake.None;
        arduino.ReadTimeout = 200;
        arduino.WriteTimeout = 200;
        arduino.Open();
        while (mRunning)
        {
            ReadFromArduino(arduino);
            WriteToArduino(arduino);
        }
        arduino.Close();
        Debug.Log("Stopping Thread");
    }

    private void ReadFromArduino(SerialPort arduino)
    {
        try
        {
            string data = arduino.ReadLine();
            lock (mReadQueue)
            {
                mReadQueue.Enqueue(data);
            }
        }
        catch (TimeoutException) { }
    }

    private void WriteToArduino(SerialPort arduino)
    {

    }

    private void Update()
    {
        lock (mReadQueue)
        {
            if (mReadQueue.Count > 0)
            {
                string data = mReadQueue.Dequeue();
                if (data == "PRESSED") PinPressed.Invoke();
                if (data == "RELEASED") PinReleased.Invoke();
            }
        }
    }
}

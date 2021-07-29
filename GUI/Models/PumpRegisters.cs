namespace Device.Pump.GUI.Models
{
    internal enum PumpRegisters
    {
        EnableMotorDriver = 32,     // U8     Enable the motor driver
        StartProtocol = 33,         // U8     Enable the defined protocol
        StepState = 34,             // U8     Control the step of the motor controller
        DirState = 35,              // U8     Control the direction of the motor controller
        SwitchForwardState = 36,    // U8     State of the forward switch limit
        SwitchReverseState = 37,    // U8     State of the reverse switch limit
        InputState = 38,            // U8     Value of input 0 pin
        SetDigitalOutputs = 39,     // U8     Set digital outputs
        ClearDigitalOutputs = 40,   // U8     Clear digital outputs
        DigitalOutput0Config = 41,  // U8     Configures which signal is connected to the digital output 0
        DigitalOutput1Config = 42,  // U8     Configures which signal is connected to the digital output 1
        DigitalInput0Config = 43,   // U8     Configuration of the digital input 0 (DI0)
        MotorMicrostep = 44,        // U8     Defines the motor microstep
        ProtocolNumberOfSteps = 45, // U16    Number of steps [1;65535]
        ProtocolFlowrate = 46,      // FLOAT  Flowrate value [0.5;2000.0]
        ProtocolStepPeriod = 47,    // U16    Period for each step in ms [1;65535]
        ProtocolVolume = 48,        // FLOAT  Volume value in uL [0.5;2000.0]
        ProtocolType = 49,          // U8     Step-based (0) or Volume-based protocol (1)
        CalibrationValue1 = 50,     // U8     Calibration value 1
        CalibrationValue2 = 51,     // U8     Calibration value 2
        EventsEnable = 52,          // U8     Enable the Events
        SetBoardType = 53,          // U8     Type of the board
        ProtocolState = 54          // U8     State of the protocol (running or stopped)
    }
}

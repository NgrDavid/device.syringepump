#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// IN00                   Description: Board digital input 0
// SW_F                   Description: Limit switch signal (forward)
// SW_R                   Description: Limit switch signal (reverse)
// EN_DRIVER_UC           Description: Enable driver from user output
// BUT_PUSH               Description: Button to push syringe
// BUT_PULL               Description: Button to pull syringe
// BUT_RESET              Description: Button to reset to initial state

#define read_IN00 read_io(PORTB, 0)             // IN00
#define read_SW_F read_io(PORTC, 4)             // SW_F
#define read_SW_R read_io(PORTC, 5)             // SW_R
#define read_EN_DRIVER_UC read_io(PORTD, 0)     // EN_DRIVER_UC
#define read_BUT_PUSH read_io(PORTD, 5)         // BUT_PUSH
#define read_BUT_PULL read_io(PORTD, 6)         // BUT_PULL
#define read_BUT_RESET read_io(PORTD, 7)        // BUT_RESET

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
// STEP                   Description: Motor controller input step
// DIR                    Description: Motor controller direction input
// MS1                    Description: Motor configuration bit 1
// MS2                    Description: Motor configuration bit 2
// MS3                    Description: Motor configuration bit 3
// EN_DRIVER              Description: Motor controller enable
// SLEEP                  Description: Motor controller sleep
// RESET                  Description: Motor controller reset
// OUT00                  Description: Board digital output 0
// OUT01                  Description: Board digital output 1
// BUF_EN                 Description: Enable signal for user controller inputs

/* STEP */
#define set_STEP set_io(PORTA, 0)
#define clr_STEP clear_io(PORTA, 0)
#define tgl_STEP toggle_io(PORTA, 0)
#define read_STEP read_io(PORTA, 0)

/* DIR */
#define set_DIR set_io(PORTA, 1)
#define clr_DIR clear_io(PORTA, 1)
#define tgl_DIR toggle_io(PORTA, 1)
#define read_DIR read_io(PORTA, 1)

/* MS1 */
#define set_MS1 set_io(PORTA, 2)
#define clr_MS1 clear_io(PORTA, 2)
#define tgl_MS1 toggle_io(PORTA, 2)
#define read_MS1 read_io(PORTA, 2)

/* MS2 */
#define set_MS2 set_io(PORTA, 3)
#define clr_MS2 clear_io(PORTA, 3)
#define tgl_MS2 toggle_io(PORTA, 3)
#define read_MS2 read_io(PORTA, 3)

/* MS3 */
#define set_MS3 set_io(PORTA, 4)
#define clr_MS3 clear_io(PORTA, 4)
#define tgl_MS3 toggle_io(PORTA, 4)
#define read_MS3 read_io(PORTA, 4)

/* EN_DRIVER */
#define set_EN_DRIVER clear_io(PORTA, 5)
#define clr_EN_DRIVER set_io(PORTA, 5)
#define tgl_EN_DRIVER toggle_io(PORTA, 5)
#define read_EN_DRIVER read_io(PORTA, 5)

/* SLEEP */
#define set_SLEEP clear_io(PORTA, 6)
#define clr_SLEEP set_io(PORTA, 6)
#define tgl_SLEEP toggle_io(PORTA, 6)
#define read_SLEEP read_io(PORTA, 6)

/* RESET */
#define set_RESET clear_io(PORTA, 7)
#define clr_RESET set_io(PORTA, 7)
#define tgl_RESET toggle_io(PORTA, 7)
#define read_RESET read_io(PORTA, 7)

/* OUT00 */
#define set_OUT00 set_io(PORTB, 1)
#define clr_OUT00 clear_io(PORTB, 1)
#define tgl_OUT00 toggle_io(PORTB, 1)
#define read_OUT00 read_io(PORTB, 1)

/* OUT01 */
#define set_OUT01 set_io(PORTB, 2)
#define clr_OUT01 clear_io(PORTB, 2)
#define tgl_OUT01 toggle_io(PORTB, 2)
#define read_OUT01 read_io(PORTB, 2)

/* BUF_EN */
#define set_BUF_EN clear_io(PORTB, 3)
#define clr_BUF_EN set_io(PORTB, 3)
#define tgl_BUF_EN toggle_io(PORTB, 3)
#define read_BUF_EN read_io(PORTB, 3)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	uint8_t REG_ENABLE_MOTOR_DRIVER;
	uint8_t REG_ENABLE_MOTOR_UC;
	uint8_t REG_START_PROTOCOL;
	uint8_t REG_STEP_STATE;
	uint8_t REG_DIR_STATE;
	uint8_t REG_SW_FORWARD_STATE;
	uint8_t REG_SW_REVERSE_STATE;
	uint8_t REG_INPUT_STATE;
	uint8_t REG_SET_DOS;
	uint8_t REG_CLEAR_DOS;
	uint8_t REG_DO0_CONFIG;
	uint8_t REG_DO1_CONFIG;
	uint8_t REG_DI0_CONFIG;
	uint8_t REG_MOTOR_MICROSTEP;
	uint16_t REG_PROTOCOL_NUMBER_STEPS;
	float REG_PROTOCOL_FLOWRATE;
	uint16_t REG_PROTOCOL_PERIOD;
	float REG_PROTOCOL_VOLUME;
	uint8_t REG_PROTOCOL_TYPE;
	uint8_t REG_CALIBRATION_VALUE_1;
	uint8_t REG_CALIBRATION_VALUE_2;
	uint8_t REG_EVT_ENABLE;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_ENABLE_MOTOR_DRIVER         32 // U8     Enable the motor driver
#define ADD_REG_ENABLE_MOTOR_UC             33 // U8     Enable the motor micro controller
#define ADD_REG_START_PROTOCOL              34 // U8     Enable the defined protocol
#define ADD_REG_STEP_STATE                  35 // U8     Control the step of the motor controller
#define ADD_REG_DIR_STATE                   36 // U8     Control the direction of the motor controller
#define ADD_REG_SW_FORWARD_STATE            37 // U8     State of the forward switch limit
#define ADD_REG_SW_REVERSE_STATE            38 // U8     State of the reverse switch limit
#define ADD_REG_INPUT_STATE                 39 // U8     Value of input 0 pin
#define ADD_REG_SET_DOS                     40 // U8     Set digital outputs
#define ADD_REG_CLEAR_DOS                   41 // U8     Clear digital outputs
#define ADD_REG_DO0_CONFIG                  42 // U8     Configures which signal is connected to the digital output 0
#define ADD_REG_DO1_CONFIG                  43 // U8     Configures which signal is connected to the digital output 1
#define ADD_REG_DI0_CONFIG                  44 // U8     Configuration of the digital input 0 (DI0)
#define ADD_REG_MOTOR_MICROSTEP             45 // U8     Defines the motor microstep
#define ADD_REG_PROTOCOL_NUMBER_STEPS       46 // U16    Number of steps [1;65535]
#define ADD_REG_PROTOCOL_FLOWRATE           47 // FLOAT  Flowrate value [0.5;2000.0]
#define ADD_REG_PROTOCOL_PERIOD             48 // U16    Period for each step in ms [1;65535]
#define ADD_REG_PROTOCOL_VOLUME             49 // FLOAT  Volume value in uL [0.5;2000.0]
#define ADD_REG_PROTOCOL_TYPE               50 // U8     Step-based (0) or Volume-based protocol (1)
#define ADD_REG_CALIBRATION_VALUE_1         51 // U8     Calibration value 1
#define ADD_REG_CALIBRATION_VALUE_2         52 // U8     Calibration value 2
#define ADD_REG_EVT_ENABLE                  53 // U8     Enable the Events

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x35
#define APP_NBYTES_OF_REG_BANK              30

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_MOTOR_ENABLE                     (1<<0)       // Enable motor when equal to 1
#define B_MOTOR_UC_ENABLE                  (1<<0)       // Enable motor when equal to 1
#define B_START_PROTOCOL                   (1<<0)       // Start the protocol when equal to 1
#define B_STEP_PIN                         (1<<0)       // Status of the STEP motor controller pin
#define B_DIR_PIN                          (1<<1)       // Status of the DIR motor controller pin
#define B_SW_F_PIN                         (1<<0)       // Status of the forward switch limit
#define B_SW_R_PIN                         (1<<1)       // Status of the reverse switch limit
#define B_DI0                              (1<<0)       // Input 0 value
#define B_SET_DO0                          (1<<0)       // Write one to this bit will put digital out 0 into High state
#define B_SET_DO1                          (1<<1)       // Write one to this bit will put digital out 1 into High state
#define B_CLR_DO0                          (1<<0)       // Write one to this bit will put digital out 0 into Low state
#define B_CLR_DO1                          (1<<1)       // Write one to this bit will put digital out 1 into Low state
#define MSK_OUT0_CONF                      (3<<0)       // Select OUT0 function
#define GM_OUT0_SOFTWARE                   (0<<0)       // Digital output controlled by software
#define GM_OUT0_SWLIMIT                    (1<<0)       // Either limits reached (register SW_FORWARD_STATE OR SW_REVERSE_STATE)
#define MSK_OUT1_CONF                      (3<<0)       // Select OUT1 function
#define GM_OUT1_SOFTWARE                   (0<<0)       // Digital output controlled by software (SET_DO1 and CLEAR_DO1)
#define GM_OUT1_DATA_SEC                   (1<<0)       // Toggles each second when is acquiring
#define GM_OUT1_STEP_STATE                 (2<<0)       // Equal to register STEP_STATE
#define MSK_DI0_CONF                       (3<<0)       // Select IN0 function
#define GM_DI0_SYNC                        (0<<0)       // Use as a pure digital input
#define GM_DI0_RISE_FALL_UPDATE_STEP       (1<<0)       // Update STEP with a rising edge
#define GM_DI0_RISE_START_PROTOCOL         (2<<0)       // Will trigger the predefined protocol on a rising edge
#define MSK_MICROSTEP                      (7<<0)       // 
#define GM_STEP_FULL                       (0<<0)       // Full step (2 phase)
#define GM_STEP_HALF                       (1<<0)       // Half step
#define GM_STEP_QUARTER                    (2<<0)       // Quarter step
#define GM_STEP_EIGHTH                     (3<<0)       // Eighth step
#define GM_STEP_SIXTEENTH                  (4<<0)       // Sixteenth step
#define B_EVT_STEP_STATE                   (1<<0)       // Event of register STEP_STATE
#define B_EVT_DIR_STATE                    (1<<1)       // Event of register DIR_STATE
#define B_EVT_SW_FORWARD_STATE             (1<<2)       // Event of register SW_FORWARD_STATE
#define B_EVT_SW_REVERSE_STATE             (1<<3)       // Event of register SW_REVERSE_STATE
#define B_EVT_INPUT_STATE                  (1<<4)       // Event of register INPUT_STATE

#endif /* _APP_REGS_H_ */
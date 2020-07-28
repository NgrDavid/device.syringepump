#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
#endif
#ifndef false
	#define false 0
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void app_read_REG_ENABLE_MOTOR_DRIVER(void);
void app_read_REG_STEP_STATE(void);
void app_read_REG_DIR_STATE(void);
void app_read_REG_SW_FORWARD_STATE(void);
void app_read_REG_SW_REVERSE_STATE(void);
void app_read_REG_INPUT_STATE(void);
void app_read_REG_SET_DOS(void);
void app_read_REG_CLEAR_DOS(void);
void app_read_REG_DO0_CONFIG(void);
void app_read_REG_DO1_CONFIG(void);
void app_read_REG_DI0_CONFIG(void);
void app_read_REG_MOTOR_MICROSTEP(void);
void app_read_REG_PROTOCOL_NUMBER_STEPS(void);
void app_read_REG_PROTOCOL_FLOWRATE(void);
void app_read_REG_PROTOCOL_VAR0(void);
void app_read_REG_PROTOCOL_VAR1(void);
void app_read_REG_PROTOCOL_VAR2(void);
void app_read_REG_EVT_ENABLE(void);

bool app_write_REG_ENABLE_MOTOR_DRIVER(void *a);
bool app_write_REG_STEP_STATE(void *a);
bool app_write_REG_DIR_STATE(void *a);
bool app_write_REG_SW_FORWARD_STATE(void *a);
bool app_write_REG_SW_REVERSE_STATE(void *a);
bool app_write_REG_INPUT_STATE(void *a);
bool app_write_REG_SET_DOS(void *a);
bool app_write_REG_CLEAR_DOS(void *a);
bool app_write_REG_DO0_CONFIG(void *a);
bool app_write_REG_DO1_CONFIG(void *a);
bool app_write_REG_DI0_CONFIG(void *a);
bool app_write_REG_MOTOR_MICROSTEP(void *a);
bool app_write_REG_PROTOCOL_NUMBER_STEPS(void *a);
bool app_write_REG_PROTOCOL_FLOWRATE(void *a);
bool app_write_REG_PROTOCOL_VAR0(void *a);
bool app_write_REG_PROTOCOL_VAR1(void *a);
bool app_write_REG_PROTOCOL_VAR2(void *a);
bool app_write_REG_EVT_ENABLE(void *a);


#endif /* _APP_FUNCTIONS_H_ */
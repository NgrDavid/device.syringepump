#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"

#include "load_motor.h"
#define F_CPU 32000000
#include <util/delay.h>

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern uint8_t app_regs_type[];
extern uint16_t app_regs_n_elements[];
extern uint8_t *app_regs_pointer[];
extern void (*app_func_rd_pointer[])(void);
extern bool (*app_func_wr_pointer[])(void*);

/************************************************************************/
/* Initialize app                                                       */
/************************************************************************/
static const uint8_t default_device_name[] = "Pump";

void hwbp_app_initialize(void)
{
    /* Define versions */
    uint8_t hwH = 1;
    uint8_t hwL = 0;
    uint8_t fwH = 1;
    uint8_t fwL = 0;
    uint8_t ass = 0;
    
   	/* Start core */
    core_func_start_core(
        1280,
        hwH, hwL,
        fwH, fwL,
        ass,
        (uint8_t*)(&app_regs),
        APP_NBYTES_OF_REG_BANK,
        APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
        default_device_name
    );
}

/************************************************************************/
/* Handle if a catastrophic error occur                                 */
/************************************************************************/
void core_callback_catastrophic_error_detected(void)
{
	clr_STEP;
	clr_DIR;
	clr_MS1;
	clr_MS2;
	clr_MS3;
	clr_EN_DRIVER;
	clr_SLEEP;
	clr_RESET;
	
	clr_OUT00;
	clr_OUT01;
	clr_BUF_EN;
}

/************************************************************************/
/* User functions                                                       */
/************************************************************************/
/* Add your functions here or load external functions if needed */

/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
void core_callback_1st_config_hw_after_boot(void)
{
	/* Initialize IOs */
	/* Don't delete this function!!! */
	init_ios();
	
	// TODO: find out if this should be done here or if it is enough to set them on core_callback_registers_were_reinitialized
	/* Initialize hardware */
	clr_BUF_EN;
	set_DIR;
	clr_MS1;
	clr_MS2;
	clr_MS3;
	clr_SLEEP;
	
	// RESET -> clear, wait 10ms, set
	set_RESET;
	_delay_ms(10);
	clr_RESET;
	
	clr_EN_DRIVER;
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	app_regs.REG_ENABLE_MOTOR_DRIVER = B_MOTOR_ENABLE;
	app_regs.REG_SET_DOS |= (B_SET_DO0 | B_SET_DO1);
	app_regs.REG_CLEAR_DOS |= (B_CLR_DO0 | B_CLR_DO1);
	app_regs.REG_DO0_CONFIG = GM_OUT0_SOFTWARE;
	app_regs.REG_DO1_CONFIG = GM_OUT1_SOFTWARE;
	app_regs.REG_DI0_CONFIG = GM_DI0_SYNC;
	app_regs.REG_MOTOR_MICROSTEP = GM_STEP_FULL;
	
	app_regs.REG_PROTOCOL_NUMBER_STEPS = 100;
	app_regs.REG_PROTOCOL_FLOWRATE = 0.5;
	
	app_regs.REG_EVT_ENABLE = (B_EVT_STEP_STATE | B_EVT_DIR_STATE | B_EVT_SW_FORWARD_STATE | B_EVT_SW_REVERSE_STATE | B_EVT_INPUT_STATE);
}

void core_callback_registers_were_reinitialized(void)
{
	/* Update registers if needed */
	app_regs.REG_ENABLE_MOTOR_DRIVER = 0;
	
	app_regs.REG_STEP_STATE = 0;
	app_regs.REG_DIR_STATE = 0;
	app_regs.REG_SW_FORWARD_STATE = 0;
	app_regs.REG_SW_REVERSE_STATE = 0;
	app_regs.REG_INPUT_STATE = 0;
	
	app_regs.REG_SET_DOS = 0;
	app_regs.REG_CLEAR_DOS = 0;
	
	// TODO: PROTOCOL VALUES should be changed?
	
	/* Update config */
	app_write_REG_DO0_CONFIG(&app_regs.REG_DO0_CONFIG);
	app_write_REG_DO1_CONFIG(&app_regs.REG_DO1_CONFIG);
	app_write_REG_DI0_CONFIG(&app_regs.REG_DI0_CONFIG);

	app_write_REG_MOTOR_MICROSTEP(&app_regs.REG_MOTOR_MICROSTEP);
	clr_EN_DRIVER;
}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update visual indicators */
	
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
	
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
void core_callback_device_to_standby(void) {}
void core_callback_device_to_active(void) {}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
uint8_t previous_IN00_value = 0;
void core_callback_t_before_exec(void) 
{
	uint8_t tmp = read_IN00;
	if( previous_IN00_value != tmp )
	{
		app_regs.REG_INPUT_STATE = tmp;
		if(app_regs.REG_EVT_ENABLE & B_EVT_INPUT_STATE)
			core_func_send_event(ADD_REG_INPUT_STATE, true);
			
		/* on DI0_SYNC mode, on IN0 transition, generate INPUT_STATE event */
		if(app_regs.REG_DI0_CONFIG & GM_DI0_SYNC)
		{
			core_func_send_event(ADD_REG_INPUT_STATE, true);
		}
		
		/* on DI0_RISE_FALL_UPDATE_STEP, on low -> high transition, generate a STEP */
		if(app_regs.REG_DI0_CONFIG & GM_DI0_RISE_FALL_UPDATE_STEP)
		{
			set_EN_DRIVER;
			
			// TODO: generate function to generate a STEP (with event)
			set_STEP;
			
			// TODO: wait to disable STEP
		}
		
		/* on DI0_RISE_START_PROTOCOL we should start the protocol defined in the PROTOCOL registers */
		if(app_regs.REG_DI0_CONFIG & GM_DI0_RISE_START_PROTOCOL)
		{
			// TODO: generate as many steps as defined in the protocol and then stop
		}
		
		previous_IN00_value = tmp;
	}
}
void core_callback_t_after_exec(void) {}
void core_callback_t_new_second(void)
{
	if((app_regs.REG_DO1_CONFIG & MSK_OUT1_CONF) == GM_DO0_DATA_SEC)
	{
		tgl_OUT01;
	}	
}
void core_callback_t_500us(void) {}
void core_callback_t_1ms(void) {
	load_motor();
}

/************************************************************************/
/* Callbacks: uart control                                              */
/************************************************************************/
void core_callback_uart_rx_before_exec(void) {}
void core_callback_uart_rx_after_exec(void) {}
void core_callback_uart_tx_before_exec(void) {}
void core_callback_uart_tx_after_exec(void) {}
void core_callback_uart_cts_before_exec(void) {}
void core_callback_uart_cts_after_exec(void) {}

/************************************************************************/
/* Callbacks: Read app register                                         */
/************************************************************************/
bool core_read_app_register(uint8_t add, uint8_t type)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;
	
	/* Receive data */
	(*app_func_rd_pointer[add-APP_REGS_ADD_MIN])();	

	/* Return success */
	return true;
}

/************************************************************************/
/* Callbacks: Write app register                                        */
/************************************************************************/
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;

	/* Check if the number of elements matches */
	if (app_regs_n_elements[add-APP_REGS_ADD_MIN] != n_elements)
		return false;

	/* Process data and return false if write is not allowed or contains errors */
	return (*app_func_wr_pointer[add-APP_REGS_ADD_MIN])(content);
}
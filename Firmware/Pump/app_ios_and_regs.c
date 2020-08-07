#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	/* Configure input pins */
	io_pin2in(&PORTB, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // IN00
	io_pin2in(&PORTC, 4, PULL_IO_UP, SENSE_IO_EDGES_BOTH);				 // SW_F
	io_pin2in(&PORTC, 5, PULL_IO_UP, SENSE_IO_EDGES_BOTH);				 // SW_R
	io_pin2in(&PORTD, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // EN_DRIVER_UC
	io_pin2in(&PORTD, 5, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // BUT_PUSH
	io_pin2in(&PORTD, 6, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // BUT_PULL
	io_pin2in(&PORTD, 7, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // BUT_RESET

	/* Configure input interrupts */
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<0), false);                 // IN00
	io_set_int(&PORTC, INT_LEVEL_LOW, 0, (1<<4), false);                 // SW_F
	io_set_int(&PORTC, INT_LEVEL_LOW, 0, (1<<5), false);                 // SW_R
	io_set_int(&PORTD, INT_LEVEL_LOW, 0, (1<<0), false);                 // EN_DRIVER_UC
	io_set_int(&PORTD, INT_LEVEL_LOW, 0, (1<<5), false);                 // BUT_PUSH
	io_set_int(&PORTD, INT_LEVEL_LOW, 0, (1<<6), false);                 // BUT_PULL
	io_set_int(&PORTD, INT_LEVEL_LOW, 0, (1<<7), false);                 // BUT_RESET

	/* Configure output pins */
	io_pin2out(&PORTA, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // STEP
	io_pin2out(&PORTA, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DIR
	io_pin2out(&PORTA, 2, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // MS1
	io_pin2out(&PORTA, 3, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // MS2
	io_pin2out(&PORTA, 4, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // MS3
	io_pin2out(&PORTA, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_DRIVER
	io_pin2out(&PORTA, 6, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // SLEEP
	io_pin2out(&PORTA, 7, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // RESET
	io_pin2out(&PORTB, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // OUT00
	io_pin2out(&PORTB, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // OUT01
	io_pin2out(&PORTB, 3, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // BUF_EN

	/* Initialize output pins */
	clr_STEP;
	clr_DIR;
	clr_MS1;
	clr_MS2;
	clr_MS3;
	set_EN_DRIVER;
	set_SLEEP;
	set_RESET;
	clr_OUT00;
	clr_OUT01;
	set_BUF_EN;
}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_FLOAT,
	TYPE_U8,
	TYPE_U16,
	TYPE_FLOAT,
	TYPE_U8
};

uint16_t app_regs_n_elements[] = {
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_ENABLE_MOTOR_DRIVER),
	(uint8_t*)(&app_regs.REG_STEP_STATE),
	(uint8_t*)(&app_regs.REG_DIR_STATE),
	(uint8_t*)(&app_regs.REG_SW_FORWARD_STATE),
	(uint8_t*)(&app_regs.REG_SW_REVERSE_STATE),
	(uint8_t*)(&app_regs.REG_INPUT_STATE),
	(uint8_t*)(&app_regs.REG_SET_DOS),
	(uint8_t*)(&app_regs.REG_CLEAR_DOS),
	(uint8_t*)(&app_regs.REG_DO0_CONFIG),
	(uint8_t*)(&app_regs.REG_DO1_CONFIG),
	(uint8_t*)(&app_regs.REG_DI0_CONFIG),
	(uint8_t*)(&app_regs.REG_MOTOR_MICROSTEP),
	(uint8_t*)(&app_regs.REG_PROTOCOL_NUMBER_STEPS),
	(uint8_t*)(&app_regs.REG_PROTOCOL_FLOWRATE),
	(uint8_t*)(&app_regs.REG_PROTOCOL_VAR0),
	(uint8_t*)(&app_regs.REG_PROTOCOL_VAR1),
	(uint8_t*)(&app_regs.REG_PROTOCOL_VAR2),
	(uint8_t*)(&app_regs.REG_EVT_ENABLE)
};
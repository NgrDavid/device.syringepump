#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern bool disable_steps;

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
// 
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
// 
// ISR(TCD1_OVF_vect, ISR_NAKED)
// 
// ISR(TCD1_CCA_vect, ISR_NAKED)

/************************************************************************/ 
/* IN00                                                                 */
/************************************************************************/
ISR(PORTB_INT0_vect, ISR_NAKED)
{
	reti();
}

/************************************************************************/ 
/* SW_F & SW_R                                                          */
/************************************************************************/
ISR(PORTC_INT0_vect, ISR_NAKED)
{
	if(!read_SW_F && !read_SW_R)
		disable_steps = true;
	else
		disable_steps = false;
	
	reti();
}

/************************************************************************/ 
/* EN_DRIVER_UC & BUT_PUSH & BUT_PULL & BUT_RESET                       */
/************************************************************************/
ISR(PORTD_INT0_vect, ISR_NAKED)
{
	reti();
}



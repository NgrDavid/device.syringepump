using Bonsai.Harp;
using System.Threading.Tasks;

namespace Harp.SyringePump
{
    /// <inheritdoc/>
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the asynchronous API to configure and interface
        /// with SyringePump devices on the specified serial port.
        /// </summary>
        /// <param name="portName">
        /// The name of the serial port used to communicate with the Harp device.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous initialization operation. The value of
        /// the <see cref="Task{TResult}.Result"/> parameter contains a new instance of
        /// the <see cref="AsyncDevice"/> class.
        /// </returns>
        public static async Task<AsyncDevice> CreateAsync(string portName)
        {
            var device = new AsyncDevice(portName);
            var whoAmI = await device.ReadWhoAmIAsync();
            if (whoAmI != Device.WhoAmI)
            {
                var errorMessage = string.Format(
                    "The device ID {1} on {0} was unexpected. Check whether a SyringePump device is connected to the specified serial port.",
                    portName, whoAmI);
                throw new HarpException(errorMessage);
            }

            return device;
        }
    }

    /// <summary>
    /// Represents an asynchronous API to configure and interface with SyringePump devices.
    /// </summary>
    public partial class AsyncDevice : Bonsai.Harp.AsyncDevice
    {
        internal AsyncDevice(string portName)
            : base(portName)
        {
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableMotorDriver register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<EnableFlag> ReadEnableMotorDriverAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableMotorDriver.Address));
            return EnableMotorDriver.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableMotorDriver register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<EnableFlag>> ReadTimestampedEnableMotorDriverAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableMotorDriver.Address));
            return EnableMotorDriver.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableMotorDriver register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableMotorDriverAsync(EnableFlag value)
        {
            var request = EnableMotorDriver.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableProtocol register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<EnableFlag> ReadEnableProtocolAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableProtocol.Address));
            return EnableProtocol.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableProtocol register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<EnableFlag>> ReadTimestampedEnableProtocolAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableProtocol.Address));
            return EnableProtocol.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableProtocol register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableProtocolAsync(EnableFlag value)
        {
            var request = EnableProtocol.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Step register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<StepState> ReadStepAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Step.Address));
            return Step.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Step register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<StepState>> ReadTimestampedStepAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Step.Address));
            return Step.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Step register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteStepAsync(StepState value)
        {
            var request = Step.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Direction register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DirectionState> ReadDirectionAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Direction.Address));
            return Direction.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Direction register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DirectionState>> ReadTimestampedDirectionAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Direction.Address));
            return Direction.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Direction register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDirectionAsync(DirectionState value)
        {
            var request = Direction.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ForwardSwitch register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ForwardSwitchState> ReadForwardSwitchAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ForwardSwitch.Address));
            return ForwardSwitch.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ForwardSwitch register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ForwardSwitchState>> ReadTimestampedForwardSwitchAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ForwardSwitch.Address));
            return ForwardSwitch.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ReverseSwitch register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ReverseSwitchState> ReadReverseSwitchAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ReverseSwitch.Address));
            return ReverseSwitch.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ReverseSwitch register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ReverseSwitchState>> ReadTimestampedReverseSwitchAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ReverseSwitch.Address));
            return ReverseSwitch.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DigitalInput register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalInputState> ReadDigitalInputAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalInput.Address));
            return DigitalInput.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DigitalInput register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalInputState>> ReadTimestampedDigitalInputAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalInput.Address));
            return DigitalInput.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DigitalOutputSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputState> ReadDigitalOutputSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputSet.Address));
            return DigitalOutputSet.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DigitalOutputSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputState>> ReadTimestampedDigitalOutputSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputSet.Address));
            return DigitalOutputSet.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DigitalOutputSet register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDigitalOutputSetAsync(DigitalOutputState value)
        {
            var request = DigitalOutputSet.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DigitalOutputClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputState> ReadDigitalOutputClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputClear.Address));
            return DigitalOutputClear.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DigitalOutputClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputState>> ReadTimestampedDigitalOutputClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputClear.Address));
            return DigitalOutputClear.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DigitalOutputClear register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDigitalOutputClearAsync(DigitalOutputState value)
        {
            var request = DigitalOutputClear.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DO0Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DO0SyncConfig> ReadDO0SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO0Sync.Address));
            return DO0Sync.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DO0Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DO0SyncConfig>> ReadTimestampedDO0SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO0Sync.Address));
            return DO0Sync.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DO0Sync register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDO0SyncAsync(DO0SyncConfig value)
        {
            var request = DO0Sync.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DO1Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DO1SyncConfig> ReadDO1SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO1Sync.Address));
            return DO1Sync.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DO1Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DO1SyncConfig>> ReadTimestampedDO1SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO1Sync.Address));
            return DO1Sync.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DO1Sync register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDO1SyncAsync(DO1SyncConfig value)
        {
            var request = DO1Sync.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DI0Trigger register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DI0TriggerConfig> ReadDI0TriggerAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DI0Trigger.Address));
            return DI0Trigger.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DI0Trigger register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DI0TriggerConfig>> ReadTimestampedDI0TriggerAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DI0Trigger.Address));
            return DI0Trigger.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DI0Trigger register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDI0TriggerAsync(DI0TriggerConfig value)
        {
            var request = DI0Trigger.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the StepMode register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<StepModeType> ReadStepModeAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StepMode.Address));
            return StepMode.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the StepMode register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<StepModeType>> ReadTimestampedStepModeAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StepMode.Address));
            return StepMode.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the StepMode register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteStepModeAsync(StepModeType value)
        {
            var request = StepMode.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ProtocolStepCount register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadProtocolStepCountAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(ProtocolStepCount.Address));
            return ProtocolStepCount.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ProtocolStepCount register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedProtocolStepCountAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(ProtocolStepCount.Address));
            return ProtocolStepCount.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ProtocolStepCount register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteProtocolStepCountAsync(ushort value)
        {
            var request = ProtocolStepCount.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ProtocolPeriod register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadProtocolPeriodAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(ProtocolPeriod.Address));
            return ProtocolPeriod.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ProtocolPeriod register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedProtocolPeriodAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(ProtocolPeriod.Address));
            return ProtocolPeriod.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ProtocolPeriod register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteProtocolPeriodAsync(ushort value)
        {
            var request = ProtocolPeriod.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableEvents register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<PumpEvents> ReadEnableEventsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableEvents.Address));
            return EnableEvents.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableEvents register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<PumpEvents>> ReadTimestampedEnableEventsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableEvents.Address));
            return EnableEvents.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableEvents register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableEventsAsync(PumpEvents value)
        {
            var request = EnableEvents.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Protocol register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ProtocolState> ReadProtocolAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Protocol.Address));
            return Protocol.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Protocol register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ProtocolState>> ReadTimestampedProtocolAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Protocol.Address));
            return Protocol.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ProtocolDirection register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ProtocolDirectionState> ReadProtocolDirectionAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ProtocolDirection.Address));
            return ProtocolDirection.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ProtocolDirection register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ProtocolDirectionState>> ReadTimestampedProtocolDirectionAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ProtocolDirection.Address));
            return ProtocolDirection.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ProtocolDirection register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteProtocolDirectionAsync(ProtocolDirectionState value)
        {
            var request = ProtocolDirection.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }
    }
}

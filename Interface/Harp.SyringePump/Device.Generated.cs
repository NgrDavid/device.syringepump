using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.SyringePump
{
    /// <summary>
    /// Generates events and processes commands for the SyringePump device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the SyringePump device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="SyringePump"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 1296;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(SyringePump);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(EnableMotorDriver) },
            { 33, typeof(EnableProtocol) },
            { 34, typeof(Step) },
            { 35, typeof(Direction) },
            { 36, typeof(ForwardSwitch) },
            { 37, typeof(ReverseSwitch) },
            { 38, typeof(DigitalInputState) },
            { 39, typeof(DigitalOutputSet) },
            { 40, typeof(DigitalOutputClear) },
            { 41, typeof(DO0Sync) },
            { 42, typeof(DO1Sync) },
            { 43, typeof(DI0Trigger) },
            { 44, typeof(StepMode) },
            { 45, typeof(ProtocolStepCount) },
            { 47, typeof(ProtocolPeriod) },
            { 52, typeof(EnableEvents) },
            { 54, typeof(Protocol) },
            { 55, typeof(ProtocolDirection) }
        };
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="SyringePump"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of SyringePump messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="SyringePump"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="SyringePump"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="SyringePump"/> device.
    /// </summary>
    /// <seealso cref="EnableMotorDriver"/>
    /// <seealso cref="EnableProtocol"/>
    /// <seealso cref="Step"/>
    /// <seealso cref="Direction"/>
    /// <seealso cref="ForwardSwitch"/>
    /// <seealso cref="ReverseSwitch"/>
    /// <seealso cref="DigitalInputState"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="StepMode"/>
    /// <seealso cref="ProtocolStepCount"/>
    /// <seealso cref="ProtocolPeriod"/>
    /// <seealso cref="EnableEvents"/>
    /// <seealso cref="Protocol"/>
    /// <seealso cref="ProtocolDirection"/>
    [XmlInclude(typeof(EnableMotorDriver))]
    [XmlInclude(typeof(EnableProtocol))]
    [XmlInclude(typeof(Step))]
    [XmlInclude(typeof(Direction))]
    [XmlInclude(typeof(ForwardSwitch))]
    [XmlInclude(typeof(ReverseSwitch))]
    [XmlInclude(typeof(DigitalInputState))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(StepMode))]
    [XmlInclude(typeof(ProtocolStepCount))]
    [XmlInclude(typeof(ProtocolPeriod))]
    [XmlInclude(typeof(EnableEvents))]
    [XmlInclude(typeof(Protocol))]
    [XmlInclude(typeof(ProtocolDirection))]
    [Description("Filters register-specific messages reported by the SyringePump device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new EnableMotorDriver();
        }

        string INamedElement.Name
        {
            get => $"{nameof(SyringePump)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the SyringePump device.
    /// </summary>
    /// <seealso cref="EnableMotorDriver"/>
    /// <seealso cref="EnableProtocol"/>
    /// <seealso cref="Step"/>
    /// <seealso cref="Direction"/>
    /// <seealso cref="ForwardSwitch"/>
    /// <seealso cref="ReverseSwitch"/>
    /// <seealso cref="DigitalInputState"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="StepMode"/>
    /// <seealso cref="ProtocolStepCount"/>
    /// <seealso cref="ProtocolPeriod"/>
    /// <seealso cref="EnableEvents"/>
    /// <seealso cref="Protocol"/>
    /// <seealso cref="ProtocolDirection"/>
    [XmlInclude(typeof(EnableMotorDriver))]
    [XmlInclude(typeof(EnableProtocol))]
    [XmlInclude(typeof(Step))]
    [XmlInclude(typeof(Direction))]
    [XmlInclude(typeof(ForwardSwitch))]
    [XmlInclude(typeof(ReverseSwitch))]
    [XmlInclude(typeof(DigitalInputState))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(StepMode))]
    [XmlInclude(typeof(ProtocolStepCount))]
    [XmlInclude(typeof(ProtocolPeriod))]
    [XmlInclude(typeof(EnableEvents))]
    [XmlInclude(typeof(Protocol))]
    [XmlInclude(typeof(ProtocolDirection))]
    [XmlInclude(typeof(TimestampedEnableMotorDriver))]
    [XmlInclude(typeof(TimestampedEnableProtocol))]
    [XmlInclude(typeof(TimestampedStep))]
    [XmlInclude(typeof(TimestampedDirection))]
    [XmlInclude(typeof(TimestampedForwardSwitch))]
    [XmlInclude(typeof(TimestampedReverseSwitch))]
    [XmlInclude(typeof(TimestampedDigitalInputState))]
    [XmlInclude(typeof(TimestampedDigitalOutputSet))]
    [XmlInclude(typeof(TimestampedDigitalOutputClear))]
    [XmlInclude(typeof(TimestampedDO0Sync))]
    [XmlInclude(typeof(TimestampedDO1Sync))]
    [XmlInclude(typeof(TimestampedDI0Trigger))]
    [XmlInclude(typeof(TimestampedStepMode))]
    [XmlInclude(typeof(TimestampedProtocolStepCount))]
    [XmlInclude(typeof(TimestampedProtocolPeriod))]
    [XmlInclude(typeof(TimestampedEnableEvents))]
    [XmlInclude(typeof(TimestampedProtocol))]
    [XmlInclude(typeof(TimestampedProtocolDirection))]
    [Description("Filters and selects specific messages reported by the SyringePump device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new EnableMotorDriver();
        }

        string INamedElement.Name => $"{nameof(SyringePump)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// SyringePump register messages.
    /// </summary>
    /// <seealso cref="EnableMotorDriver"/>
    /// <seealso cref="EnableProtocol"/>
    /// <seealso cref="Step"/>
    /// <seealso cref="Direction"/>
    /// <seealso cref="ForwardSwitch"/>
    /// <seealso cref="ReverseSwitch"/>
    /// <seealso cref="DigitalInputState"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="StepMode"/>
    /// <seealso cref="ProtocolStepCount"/>
    /// <seealso cref="ProtocolPeriod"/>
    /// <seealso cref="EnableEvents"/>
    /// <seealso cref="Protocol"/>
    /// <seealso cref="ProtocolDirection"/>
    [XmlInclude(typeof(EnableMotorDriver))]
    [XmlInclude(typeof(EnableProtocol))]
    [XmlInclude(typeof(Step))]
    [XmlInclude(typeof(Direction))]
    [XmlInclude(typeof(ForwardSwitch))]
    [XmlInclude(typeof(ReverseSwitch))]
    [XmlInclude(typeof(DigitalInputState))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(StepMode))]
    [XmlInclude(typeof(ProtocolStepCount))]
    [XmlInclude(typeof(ProtocolPeriod))]
    [XmlInclude(typeof(EnableEvents))]
    [XmlInclude(typeof(Protocol))]
    [XmlInclude(typeof(ProtocolDirection))]
    [Description("Formats a sequence of values as specific SyringePump register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new EnableMotorDriver();
        }

        string INamedElement.Name => $"{nameof(SyringePump)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that enables the motor driver.
    /// </summary>
    [Description("Enables the motor driver.")]
    public partial class EnableMotorDriver
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableMotorDriver"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableMotorDriver"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableMotorDriver"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableMotorDriver"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EnableFlag GetPayload(HarpMessage message)
        {
            return (EnableFlag)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableMotorDriver"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EnableFlag)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableMotorDriver"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableMotorDriver"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableMotorDriver"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableMotorDriver"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableMotorDriver register.
    /// </summary>
    /// <seealso cref="EnableMotorDriver"/>
    [Description("Filters and selects timestamped messages from the EnableMotorDriver register.")]
    public partial class TimestampedEnableMotorDriver
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableMotorDriver"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableMotorDriver.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableMotorDriver"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetPayload(HarpMessage message)
        {
            return EnableMotorDriver.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that enables the currently defined protocol.
    /// </summary>
    [Description("Enables the currently defined protocol.")]
    public partial class EnableProtocol
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableProtocol"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableProtocol"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableProtocol"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableProtocol"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EnableFlag GetPayload(HarpMessage message)
        {
            return (EnableFlag)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableProtocol"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EnableFlag)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableProtocol"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableProtocol"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableProtocol"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableProtocol"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableProtocol register.
    /// </summary>
    /// <seealso cref="EnableProtocol"/>
    [Description("Filters and selects timestamped messages from the EnableProtocol register.")]
    public partial class TimestampedEnableProtocol
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableProtocol"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableProtocol.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableProtocol"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetPayload(HarpMessage message)
        {
            return EnableProtocol.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that status of the STEP motor controller pin.
    /// </summary>
    [Description("Status of the STEP motor controller pin.")]
    public partial class Step
    {
        /// <summary>
        /// Represents the address of the <see cref="Step"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="Step"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Step"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Step"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static StepState GetPayload(HarpMessage message)
        {
            return (StepState)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Step"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<StepState> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((StepState)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Step"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Step"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, StepState value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Step"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Step"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, StepState value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Step register.
    /// </summary>
    /// <seealso cref="Step"/>
    [Description("Filters and selects timestamped messages from the Step register.")]
    public partial class TimestampedStep
    {
        /// <summary>
        /// Represents the address of the <see cref="Step"/> register. This field is constant.
        /// </summary>
        public const int Address = Step.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Step"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<StepState> GetPayload(HarpMessage message)
        {
            return Step.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that status of the DIR motor controller pin.
    /// </summary>
    [Description("Status of the DIR motor controller pin.")]
    public partial class Direction
    {
        /// <summary>
        /// Represents the address of the <see cref="Direction"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="Direction"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Direction"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Direction"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DirectionState GetPayload(HarpMessage message)
        {
            return (DirectionState)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Direction"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DirectionState> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DirectionState)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Direction"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Direction"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DirectionState value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Direction"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Direction"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DirectionState value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Direction register.
    /// </summary>
    /// <seealso cref="Direction"/>
    [Description("Filters and selects timestamped messages from the Direction register.")]
    public partial class TimestampedDirection
    {
        /// <summary>
        /// Represents the address of the <see cref="Direction"/> register. This field is constant.
        /// </summary>
        public const int Address = Direction.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Direction"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DirectionState> GetPayload(HarpMessage message)
        {
            return Direction.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that status of the forward limit switch.
    /// </summary>
    [Description("Status of the forward limit switch.")]
    public partial class ForwardSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="ForwardSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="ForwardSwitch"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ForwardSwitch"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ForwardSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ForwardSwitchState GetPayload(HarpMessage message)
        {
            return (ForwardSwitchState)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ForwardSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ForwardSwitchState> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ForwardSwitchState)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ForwardSwitch"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ForwardSwitch"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ForwardSwitchState value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ForwardSwitch"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ForwardSwitch"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ForwardSwitchState value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ForwardSwitch register.
    /// </summary>
    /// <seealso cref="ForwardSwitch"/>
    [Description("Filters and selects timestamped messages from the ForwardSwitch register.")]
    public partial class TimestampedForwardSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="ForwardSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = ForwardSwitch.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ForwardSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ForwardSwitchState> GetPayload(HarpMessage message)
        {
            return ForwardSwitch.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that status of the reverse limit switch.
    /// </summary>
    [Description("Status of the reverse limit switch.")]
    public partial class ReverseSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="ReverseSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="ReverseSwitch"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ReverseSwitch"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ReverseSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ReverseSwitchState GetPayload(HarpMessage message)
        {
            return (ReverseSwitchState)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ReverseSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ReverseSwitchState> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ReverseSwitchState)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ReverseSwitch"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ReverseSwitch"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ReverseSwitchState value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ReverseSwitch"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ReverseSwitch"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ReverseSwitchState value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ReverseSwitch register.
    /// </summary>
    /// <seealso cref="ReverseSwitch"/>
    [Description("Filters and selects timestamped messages from the ReverseSwitch register.")]
    public partial class TimestampedReverseSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="ReverseSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = ReverseSwitch.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ReverseSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ReverseSwitchState> GetPayload(HarpMessage message)
        {
            return ReverseSwitch.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that status of the digital input pin.
    /// </summary>
    [Description("Status of the digital input pin.")]
    public partial class DigitalInputState
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalInputState"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalInputState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalInputState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalInputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalInputs GetPayload(HarpMessage message)
        {
            return (DigitalInputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalInputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalInputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalInputState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalInputState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalInputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalInputState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalInputState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalInputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalInputState register.
    /// </summary>
    /// <seealso cref="DigitalInputState"/>
    [Description("Filters and selects timestamped messages from the DigitalInputState register.")]
    public partial class TimestampedDigitalInputState
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalInputState"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalInputState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalInputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputs> GetPayload(HarpMessage message)
        {
            return DigitalInputState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that set the specified digital output lines.
    /// </summary>
    [Description("Set the specified digital output lines.")]
    public partial class DigitalOutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalOutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalOutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalOutputSet"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputSet"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalOutputSet"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputSet"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalOutputSet register.
    /// </summary>
    /// <seealso cref="DigitalOutputSet"/>
    [Description("Filters and selects timestamped messages from the DigitalOutputSet register.")]
    public partial class TimestampedDigitalOutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalOutputSet.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalOutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return DigitalOutputSet.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that clear the specified digital output lines.
    /// </summary>
    [Description("Clear the specified digital output lines.")]
    public partial class DigitalOutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalOutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalOutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalOutputClear"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputClear"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalOutputClear"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputClear"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalOutputClear register.
    /// </summary>
    /// <seealso cref="DigitalOutputClear"/>
    [Description("Filters and selects timestamped messages from the DigitalOutputClear register.")]
    public partial class TimestampedDigitalOutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalOutputClear.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalOutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return DigitalOutputClear.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configures which signal is mimicked in the digital output 0.
    /// </summary>
    [Description("Configures which signal is mimicked in the digital output 0.")]
    public partial class DO0Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = 41;

        /// <summary>
        /// Represents the payload type of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DO0SyncConfig GetPayload(HarpMessage message)
        {
            return (DO0SyncConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO0SyncConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DO0SyncConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DO0Sync"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO0Sync"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DO0SyncConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DO0Sync"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO0Sync"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DO0SyncConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DO0Sync register.
    /// </summary>
    /// <seealso cref="DO0Sync"/>
    [Description("Filters and selects timestamped messages from the DO0Sync register.")]
    public partial class TimestampedDO0Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = DO0Sync.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO0SyncConfig> GetPayload(HarpMessage message)
        {
            return DO0Sync.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configures which signal is mimicked in the digital output 1.
    /// </summary>
    [Description("Configures which signal is mimicked in the digital output 1.")]
    public partial class DO1Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO1Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = 42;

        /// <summary>
        /// Represents the payload type of the <see cref="DO1Sync"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DO1Sync"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DO1Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DO1SyncConfig GetPayload(HarpMessage message)
        {
            return (DO1SyncConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DO1Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO1SyncConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DO1SyncConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DO1Sync"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO1Sync"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DO1SyncConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DO1Sync"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO1Sync"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DO1SyncConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DO1Sync register.
    /// </summary>
    /// <seealso cref="DO1Sync"/>
    [Description("Filters and selects timestamped messages from the DO1Sync register.")]
    public partial class TimestampedDO1Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO1Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = DO1Sync.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DO1Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO1SyncConfig> GetPayload(HarpMessage message)
        {
            return DO1Sync.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configures the callback function triggered when digital input is triggered.
    /// </summary>
    [Description("Configures the callback function triggered when digital input is triggered.")]
    public partial class DI0Trigger
    {
        /// <summary>
        /// Represents the address of the <see cref="DI0Trigger"/> register. This field is constant.
        /// </summary>
        public const int Address = 43;

        /// <summary>
        /// Represents the payload type of the <see cref="DI0Trigger"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DI0Trigger"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DI0Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DI0TriggerConfig GetPayload(HarpMessage message)
        {
            return (DI0TriggerConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DI0Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DI0TriggerConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DI0TriggerConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DI0Trigger"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DI0Trigger"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DI0TriggerConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DI0Trigger"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DI0Trigger"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DI0TriggerConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DI0Trigger register.
    /// </summary>
    /// <seealso cref="DI0Trigger"/>
    [Description("Filters and selects timestamped messages from the DI0Trigger register.")]
    public partial class TimestampedDI0Trigger
    {
        /// <summary>
        /// Represents the address of the <see cref="DI0Trigger"/> register. This field is constant.
        /// </summary>
        public const int Address = DI0Trigger.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DI0Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DI0TriggerConfig> GetPayload(HarpMessage message)
        {
            return DI0Trigger.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the motor step mode from a list of available types.
    /// </summary>
    [Description("Sets the motor step mode from a list of available types.")]
    public partial class StepMode
    {
        /// <summary>
        /// Represents the address of the <see cref="StepMode"/> register. This field is constant.
        /// </summary>
        public const int Address = 44;

        /// <summary>
        /// Represents the payload type of the <see cref="StepMode"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="StepMode"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="StepMode"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static StepModeType GetPayload(HarpMessage message)
        {
            return (StepModeType)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="StepMode"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<StepModeType> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((StepModeType)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="StepMode"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StepMode"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, StepModeType value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="StepMode"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StepMode"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, StepModeType value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// StepMode register.
    /// </summary>
    /// <seealso cref="StepMode"/>
    [Description("Filters and selects timestamped messages from the StepMode register.")]
    public partial class TimestampedStepMode
    {
        /// <summary>
        /// Represents the address of the <see cref="StepMode"/> register. This field is constant.
        /// </summary>
        public const int Address = StepMode.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="StepMode"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<StepModeType> GetPayload(HarpMessage message)
        {
            return StepMode.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the number of steps to be executed in the current protocol.
    /// </summary>
    [Description("Sets the number of steps to be executed in the current protocol.")]
    public partial class ProtocolStepCount
    {
        /// <summary>
        /// Represents the address of the <see cref="ProtocolStepCount"/> register. This field is constant.
        /// </summary>
        public const int Address = 45;

        /// <summary>
        /// Represents the payload type of the <see cref="ProtocolStepCount"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="ProtocolStepCount"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ProtocolStepCount"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ProtocolStepCount"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ProtocolStepCount"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ProtocolStepCount"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ProtocolStepCount"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ProtocolStepCount"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ProtocolStepCount register.
    /// </summary>
    /// <seealso cref="ProtocolStepCount"/>
    [Description("Filters and selects timestamped messages from the ProtocolStepCount register.")]
    public partial class TimestampedProtocolStepCount
    {
        /// <summary>
        /// Represents the address of the <see cref="ProtocolStepCount"/> register. This field is constant.
        /// </summary>
        public const int Address = ProtocolStepCount.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ProtocolStepCount"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return ProtocolStepCount.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the period, in ms, of of each step in the protocol.
    /// </summary>
    [Description("Sets the period, in ms, of of each step in the protocol.")]
    public partial class ProtocolPeriod
    {
        /// <summary>
        /// Represents the address of the <see cref="ProtocolPeriod"/> register. This field is constant.
        /// </summary>
        public const int Address = 47;

        /// <summary>
        /// Represents the payload type of the <see cref="ProtocolPeriod"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="ProtocolPeriod"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ProtocolPeriod"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ProtocolPeriod"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ProtocolPeriod"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ProtocolPeriod"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ProtocolPeriod"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ProtocolPeriod"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ProtocolPeriod register.
    /// </summary>
    /// <seealso cref="ProtocolPeriod"/>
    [Description("Filters and selects timestamped messages from the ProtocolPeriod register.")]
    public partial class TimestampedProtocolPeriod
    {
        /// <summary>
        /// Represents the address of the <see cref="ProtocolPeriod"/> register. This field is constant.
        /// </summary>
        public const int Address = ProtocolPeriod.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ProtocolPeriod"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return ProtocolPeriod.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies all the active events in the device.
    /// </summary>
    [Description("Specifies all the active events in the device.")]
    public partial class EnableEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = 52;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static PumpEvents GetPayload(HarpMessage message)
        {
            return (PumpEvents)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<PumpEvents> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((PumpEvents)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableEvents"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableEvents"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, PumpEvents value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableEvents"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableEvents"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, PumpEvents value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableEvents register.
    /// </summary>
    /// <seealso cref="EnableEvents"/>
    [Description("Filters and selects timestamped messages from the EnableEvents register.")]
    public partial class TimestampedEnableEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableEvents.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<PumpEvents> GetPayload(HarpMessage message)
        {
            return EnableEvents.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that status of the protocol execution.
    /// </summary>
    [Description("Status of the protocol execution.")]
    public partial class Protocol
    {
        /// <summary>
        /// Represents the address of the <see cref="Protocol"/> register. This field is constant.
        /// </summary>
        public const int Address = 54;

        /// <summary>
        /// Represents the payload type of the <see cref="Protocol"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Protocol"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Protocol"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ProtocolState GetPayload(HarpMessage message)
        {
            return (ProtocolState)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Protocol"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ProtocolState> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ProtocolState)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Protocol"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Protocol"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ProtocolState value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Protocol"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Protocol"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ProtocolState value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Protocol register.
    /// </summary>
    /// <seealso cref="Protocol"/>
    [Description("Filters and selects timestamped messages from the Protocol register.")]
    public partial class TimestampedProtocol
    {
        /// <summary>
        /// Represents the address of the <see cref="Protocol"/> register. This field is constant.
        /// </summary>
        public const int Address = Protocol.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Protocol"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ProtocolState> GetPayload(HarpMessage message)
        {
            return Protocol.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the direction of the protocol execution.
    /// </summary>
    [Description("Sets the direction of the protocol execution.")]
    public partial class ProtocolDirection
    {
        /// <summary>
        /// Represents the address of the <see cref="ProtocolDirection"/> register. This field is constant.
        /// </summary>
        public const int Address = 55;

        /// <summary>
        /// Represents the payload type of the <see cref="ProtocolDirection"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ProtocolDirection"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ProtocolDirection"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ProtocolDirectionState GetPayload(HarpMessage message)
        {
            return (ProtocolDirectionState)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ProtocolDirection"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ProtocolDirectionState> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ProtocolDirectionState)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ProtocolDirection"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ProtocolDirection"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ProtocolDirectionState value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ProtocolDirection"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ProtocolDirection"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ProtocolDirectionState value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ProtocolDirection register.
    /// </summary>
    /// <seealso cref="ProtocolDirection"/>
    [Description("Filters and selects timestamped messages from the ProtocolDirection register.")]
    public partial class TimestampedProtocolDirection
    {
        /// <summary>
        /// Represents the address of the <see cref="ProtocolDirection"/> register. This field is constant.
        /// </summary>
        public const int Address = ProtocolDirection.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ProtocolDirection"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ProtocolDirectionState> GetPayload(HarpMessage message)
        {
            return ProtocolDirection.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// SyringePump device.
    /// </summary>
    /// <seealso cref="CreateEnableMotorDriverPayload"/>
    /// <seealso cref="CreateEnableProtocolPayload"/>
    /// <seealso cref="CreateStepPayload"/>
    /// <seealso cref="CreateDirectionPayload"/>
    /// <seealso cref="CreateForwardSwitchPayload"/>
    /// <seealso cref="CreateReverseSwitchPayload"/>
    /// <seealso cref="CreateDigitalInputStatePayload"/>
    /// <seealso cref="CreateDigitalOutputSetPayload"/>
    /// <seealso cref="CreateDigitalOutputClearPayload"/>
    /// <seealso cref="CreateDO0SyncPayload"/>
    /// <seealso cref="CreateDO1SyncPayload"/>
    /// <seealso cref="CreateDI0TriggerPayload"/>
    /// <seealso cref="CreateStepModePayload"/>
    /// <seealso cref="CreateProtocolStepCountPayload"/>
    /// <seealso cref="CreateProtocolPeriodPayload"/>
    /// <seealso cref="CreateEnableEventsPayload"/>
    /// <seealso cref="CreateProtocolPayload"/>
    /// <seealso cref="CreateProtocolDirectionPayload"/>
    [XmlInclude(typeof(CreateEnableMotorDriverPayload))]
    [XmlInclude(typeof(CreateEnableProtocolPayload))]
    [XmlInclude(typeof(CreateStepPayload))]
    [XmlInclude(typeof(CreateDirectionPayload))]
    [XmlInclude(typeof(CreateForwardSwitchPayload))]
    [XmlInclude(typeof(CreateReverseSwitchPayload))]
    [XmlInclude(typeof(CreateDigitalInputStatePayload))]
    [XmlInclude(typeof(CreateDigitalOutputSetPayload))]
    [XmlInclude(typeof(CreateDigitalOutputClearPayload))]
    [XmlInclude(typeof(CreateDO0SyncPayload))]
    [XmlInclude(typeof(CreateDO1SyncPayload))]
    [XmlInclude(typeof(CreateDI0TriggerPayload))]
    [XmlInclude(typeof(CreateStepModePayload))]
    [XmlInclude(typeof(CreateProtocolStepCountPayload))]
    [XmlInclude(typeof(CreateProtocolPeriodPayload))]
    [XmlInclude(typeof(CreateEnableEventsPayload))]
    [XmlInclude(typeof(CreateProtocolPayload))]
    [XmlInclude(typeof(CreateProtocolDirectionPayload))]
    [XmlInclude(typeof(CreateTimestampedEnableMotorDriverPayload))]
    [XmlInclude(typeof(CreateTimestampedEnableProtocolPayload))]
    [XmlInclude(typeof(CreateTimestampedStepPayload))]
    [XmlInclude(typeof(CreateTimestampedDirectionPayload))]
    [XmlInclude(typeof(CreateTimestampedForwardSwitchPayload))]
    [XmlInclude(typeof(CreateTimestampedReverseSwitchPayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalInputStatePayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputSetPayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputClearPayload))]
    [XmlInclude(typeof(CreateTimestampedDO0SyncPayload))]
    [XmlInclude(typeof(CreateTimestampedDO1SyncPayload))]
    [XmlInclude(typeof(CreateTimestampedDI0TriggerPayload))]
    [XmlInclude(typeof(CreateTimestampedStepModePayload))]
    [XmlInclude(typeof(CreateTimestampedProtocolStepCountPayload))]
    [XmlInclude(typeof(CreateTimestampedProtocolPeriodPayload))]
    [XmlInclude(typeof(CreateTimestampedEnableEventsPayload))]
    [XmlInclude(typeof(CreateTimestampedProtocolPayload))]
    [XmlInclude(typeof(CreateTimestampedProtocolDirectionPayload))]
    [Description("Creates standard message payloads for the SyringePump device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateEnableMotorDriverPayload();
        }

        string INamedElement.Name => $"{nameof(SyringePump)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that enables the motor driver.
    /// </summary>
    [DisplayName("EnableMotorDriverPayload")]
    [Description("Creates a message payload that enables the motor driver.")]
    public partial class CreateEnableMotorDriverPayload
    {
        /// <summary>
        /// Gets or sets the value that enables the motor driver.
        /// </summary>
        [Description("The value that enables the motor driver.")]
        public EnableFlag EnableMotorDriver { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableMotorDriver register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EnableFlag GetPayload()
        {
            return EnableMotorDriver;
        }

        /// <summary>
        /// Creates a message that enables the motor driver.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableMotorDriver register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.EnableMotorDriver.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enables the motor driver.
    /// </summary>
    [DisplayName("TimestampedEnableMotorDriverPayload")]
    [Description("Creates a timestamped message payload that enables the motor driver.")]
    public partial class CreateTimestampedEnableMotorDriverPayload : CreateEnableMotorDriverPayload
    {
        /// <summary>
        /// Creates a timestamped message that enables the motor driver.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableMotorDriver register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.EnableMotorDriver.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that enables the currently defined protocol.
    /// </summary>
    [DisplayName("EnableProtocolPayload")]
    [Description("Creates a message payload that enables the currently defined protocol.")]
    public partial class CreateEnableProtocolPayload
    {
        /// <summary>
        /// Gets or sets the value that enables the currently defined protocol.
        /// </summary>
        [Description("The value that enables the currently defined protocol.")]
        public EnableFlag EnableProtocol { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableProtocol register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EnableFlag GetPayload()
        {
            return EnableProtocol;
        }

        /// <summary>
        /// Creates a message that enables the currently defined protocol.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableProtocol register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.EnableProtocol.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enables the currently defined protocol.
    /// </summary>
    [DisplayName("TimestampedEnableProtocolPayload")]
    [Description("Creates a timestamped message payload that enables the currently defined protocol.")]
    public partial class CreateTimestampedEnableProtocolPayload : CreateEnableProtocolPayload
    {
        /// <summary>
        /// Creates a timestamped message that enables the currently defined protocol.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableProtocol register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.EnableProtocol.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that status of the STEP motor controller pin.
    /// </summary>
    [DisplayName("StepPayload")]
    [Description("Creates a message payload that status of the STEP motor controller pin.")]
    public partial class CreateStepPayload
    {
        /// <summary>
        /// Gets or sets the value that status of the STEP motor controller pin.
        /// </summary>
        [Description("The value that status of the STEP motor controller pin.")]
        public StepState Step { get; set; }

        /// <summary>
        /// Creates a message payload for the Step register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public StepState GetPayload()
        {
            return Step;
        }

        /// <summary>
        /// Creates a message that status of the STEP motor controller pin.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Step register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.Step.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that status of the STEP motor controller pin.
    /// </summary>
    [DisplayName("TimestampedStepPayload")]
    [Description("Creates a timestamped message payload that status of the STEP motor controller pin.")]
    public partial class CreateTimestampedStepPayload : CreateStepPayload
    {
        /// <summary>
        /// Creates a timestamped message that status of the STEP motor controller pin.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Step register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.Step.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that status of the DIR motor controller pin.
    /// </summary>
    [DisplayName("DirectionPayload")]
    [Description("Creates a message payload that status of the DIR motor controller pin.")]
    public partial class CreateDirectionPayload
    {
        /// <summary>
        /// Gets or sets the value that status of the DIR motor controller pin.
        /// </summary>
        [Description("The value that status of the DIR motor controller pin.")]
        public DirectionState Direction { get; set; }

        /// <summary>
        /// Creates a message payload for the Direction register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DirectionState GetPayload()
        {
            return Direction;
        }

        /// <summary>
        /// Creates a message that status of the DIR motor controller pin.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Direction register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.Direction.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that status of the DIR motor controller pin.
    /// </summary>
    [DisplayName("TimestampedDirectionPayload")]
    [Description("Creates a timestamped message payload that status of the DIR motor controller pin.")]
    public partial class CreateTimestampedDirectionPayload : CreateDirectionPayload
    {
        /// <summary>
        /// Creates a timestamped message that status of the DIR motor controller pin.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Direction register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.Direction.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that status of the forward limit switch.
    /// </summary>
    [DisplayName("ForwardSwitchPayload")]
    [Description("Creates a message payload that status of the forward limit switch.")]
    public partial class CreateForwardSwitchPayload
    {
        /// <summary>
        /// Gets or sets the value that status of the forward limit switch.
        /// </summary>
        [Description("The value that status of the forward limit switch.")]
        public ForwardSwitchState ForwardSwitch { get; set; }

        /// <summary>
        /// Creates a message payload for the ForwardSwitch register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ForwardSwitchState GetPayload()
        {
            return ForwardSwitch;
        }

        /// <summary>
        /// Creates a message that status of the forward limit switch.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ForwardSwitch register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.ForwardSwitch.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that status of the forward limit switch.
    /// </summary>
    [DisplayName("TimestampedForwardSwitchPayload")]
    [Description("Creates a timestamped message payload that status of the forward limit switch.")]
    public partial class CreateTimestampedForwardSwitchPayload : CreateForwardSwitchPayload
    {
        /// <summary>
        /// Creates a timestamped message that status of the forward limit switch.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ForwardSwitch register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.ForwardSwitch.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that status of the reverse limit switch.
    /// </summary>
    [DisplayName("ReverseSwitchPayload")]
    [Description("Creates a message payload that status of the reverse limit switch.")]
    public partial class CreateReverseSwitchPayload
    {
        /// <summary>
        /// Gets or sets the value that status of the reverse limit switch.
        /// </summary>
        [Description("The value that status of the reverse limit switch.")]
        public ReverseSwitchState ReverseSwitch { get; set; }

        /// <summary>
        /// Creates a message payload for the ReverseSwitch register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ReverseSwitchState GetPayload()
        {
            return ReverseSwitch;
        }

        /// <summary>
        /// Creates a message that status of the reverse limit switch.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ReverseSwitch register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.ReverseSwitch.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that status of the reverse limit switch.
    /// </summary>
    [DisplayName("TimestampedReverseSwitchPayload")]
    [Description("Creates a timestamped message payload that status of the reverse limit switch.")]
    public partial class CreateTimestampedReverseSwitchPayload : CreateReverseSwitchPayload
    {
        /// <summary>
        /// Creates a timestamped message that status of the reverse limit switch.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ReverseSwitch register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.ReverseSwitch.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that status of the digital input pin.
    /// </summary>
    [DisplayName("DigitalInputStatePayload")]
    [Description("Creates a message payload that status of the digital input pin.")]
    public partial class CreateDigitalInputStatePayload
    {
        /// <summary>
        /// Gets or sets the value that status of the digital input pin.
        /// </summary>
        [Description("The value that status of the digital input pin.")]
        public DigitalInputs DigitalInputState { get; set; }

        /// <summary>
        /// Creates a message payload for the DigitalInputState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalInputs GetPayload()
        {
            return DigitalInputState;
        }

        /// <summary>
        /// Creates a message that status of the digital input pin.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DigitalInputState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.DigitalInputState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that status of the digital input pin.
    /// </summary>
    [DisplayName("TimestampedDigitalInputStatePayload")]
    [Description("Creates a timestamped message payload that status of the digital input pin.")]
    public partial class CreateTimestampedDigitalInputStatePayload : CreateDigitalInputStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that status of the digital input pin.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DigitalInputState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.DigitalInputState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("DigitalOutputSetPayload")]
    [Description("Creates a message payload that set the specified digital output lines.")]
    public partial class CreateDigitalOutputSetPayload
    {
        /// <summary>
        /// Gets or sets the value that set the specified digital output lines.
        /// </summary>
        [Description("The value that set the specified digital output lines.")]
        public DigitalOutputs DigitalOutputSet { get; set; }

        /// <summary>
        /// Creates a message payload for the DigitalOutputSet register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return DigitalOutputSet;
        }

        /// <summary>
        /// Creates a message that set the specified digital output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DigitalOutputSet register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.DigitalOutputSet.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputSetPayload")]
    [Description("Creates a timestamped message payload that set the specified digital output lines.")]
    public partial class CreateTimestampedDigitalOutputSetPayload : CreateDigitalOutputSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that set the specified digital output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DigitalOutputSet register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.DigitalOutputSet.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that clear the specified digital output lines.
    /// </summary>
    [DisplayName("DigitalOutputClearPayload")]
    [Description("Creates a message payload that clear the specified digital output lines.")]
    public partial class CreateDigitalOutputClearPayload
    {
        /// <summary>
        /// Gets or sets the value that clear the specified digital output lines.
        /// </summary>
        [Description("The value that clear the specified digital output lines.")]
        public DigitalOutputs DigitalOutputClear { get; set; }

        /// <summary>
        /// Creates a message payload for the DigitalOutputClear register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return DigitalOutputClear;
        }

        /// <summary>
        /// Creates a message that clear the specified digital output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DigitalOutputClear register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.DigitalOutputClear.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that clear the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputClearPayload")]
    [Description("Creates a timestamped message payload that clear the specified digital output lines.")]
    public partial class CreateTimestampedDigitalOutputClearPayload : CreateDigitalOutputClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that clear the specified digital output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DigitalOutputClear register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.DigitalOutputClear.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configures which signal is mimicked in the digital output 0.
    /// </summary>
    [DisplayName("DO0SyncPayload")]
    [Description("Creates a message payload that configures which signal is mimicked in the digital output 0.")]
    public partial class CreateDO0SyncPayload
    {
        /// <summary>
        /// Gets or sets the value that configures which signal is mimicked in the digital output 0.
        /// </summary>
        [Description("The value that configures which signal is mimicked in the digital output 0.")]
        public DO0SyncConfig DO0Sync { get; set; }

        /// <summary>
        /// Creates a message payload for the DO0Sync register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DO0SyncConfig GetPayload()
        {
            return DO0Sync;
        }

        /// <summary>
        /// Creates a message that configures which signal is mimicked in the digital output 0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DO0Sync register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.DO0Sync.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configures which signal is mimicked in the digital output 0.
    /// </summary>
    [DisplayName("TimestampedDO0SyncPayload")]
    [Description("Creates a timestamped message payload that configures which signal is mimicked in the digital output 0.")]
    public partial class CreateTimestampedDO0SyncPayload : CreateDO0SyncPayload
    {
        /// <summary>
        /// Creates a timestamped message that configures which signal is mimicked in the digital output 0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DO0Sync register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.DO0Sync.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configures which signal is mimicked in the digital output 1.
    /// </summary>
    [DisplayName("DO1SyncPayload")]
    [Description("Creates a message payload that configures which signal is mimicked in the digital output 1.")]
    public partial class CreateDO1SyncPayload
    {
        /// <summary>
        /// Gets or sets the value that configures which signal is mimicked in the digital output 1.
        /// </summary>
        [Description("The value that configures which signal is mimicked in the digital output 1.")]
        public DO1SyncConfig DO1Sync { get; set; }

        /// <summary>
        /// Creates a message payload for the DO1Sync register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DO1SyncConfig GetPayload()
        {
            return DO1Sync;
        }

        /// <summary>
        /// Creates a message that configures which signal is mimicked in the digital output 1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DO1Sync register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.DO1Sync.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configures which signal is mimicked in the digital output 1.
    /// </summary>
    [DisplayName("TimestampedDO1SyncPayload")]
    [Description("Creates a timestamped message payload that configures which signal is mimicked in the digital output 1.")]
    public partial class CreateTimestampedDO1SyncPayload : CreateDO1SyncPayload
    {
        /// <summary>
        /// Creates a timestamped message that configures which signal is mimicked in the digital output 1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DO1Sync register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.DO1Sync.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configures the callback function triggered when digital input is triggered.
    /// </summary>
    [DisplayName("DI0TriggerPayload")]
    [Description("Creates a message payload that configures the callback function triggered when digital input is triggered.")]
    public partial class CreateDI0TriggerPayload
    {
        /// <summary>
        /// Gets or sets the value that configures the callback function triggered when digital input is triggered.
        /// </summary>
        [Description("The value that configures the callback function triggered when digital input is triggered.")]
        public DI0TriggerConfig DI0Trigger { get; set; }

        /// <summary>
        /// Creates a message payload for the DI0Trigger register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DI0TriggerConfig GetPayload()
        {
            return DI0Trigger;
        }

        /// <summary>
        /// Creates a message that configures the callback function triggered when digital input is triggered.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DI0Trigger register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.DI0Trigger.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configures the callback function triggered when digital input is triggered.
    /// </summary>
    [DisplayName("TimestampedDI0TriggerPayload")]
    [Description("Creates a timestamped message payload that configures the callback function triggered when digital input is triggered.")]
    public partial class CreateTimestampedDI0TriggerPayload : CreateDI0TriggerPayload
    {
        /// <summary>
        /// Creates a timestamped message that configures the callback function triggered when digital input is triggered.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DI0Trigger register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.DI0Trigger.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the motor step mode from a list of available types.
    /// </summary>
    [DisplayName("StepModePayload")]
    [Description("Creates a message payload that sets the motor step mode from a list of available types.")]
    public partial class CreateStepModePayload
    {
        /// <summary>
        /// Gets or sets the value that sets the motor step mode from a list of available types.
        /// </summary>
        [Description("The value that sets the motor step mode from a list of available types.")]
        public StepModeType StepMode { get; set; }

        /// <summary>
        /// Creates a message payload for the StepMode register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public StepModeType GetPayload()
        {
            return StepMode;
        }

        /// <summary>
        /// Creates a message that sets the motor step mode from a list of available types.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the StepMode register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.StepMode.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the motor step mode from a list of available types.
    /// </summary>
    [DisplayName("TimestampedStepModePayload")]
    [Description("Creates a timestamped message payload that sets the motor step mode from a list of available types.")]
    public partial class CreateTimestampedStepModePayload : CreateStepModePayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the motor step mode from a list of available types.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the StepMode register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.StepMode.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the number of steps to be executed in the current protocol.
    /// </summary>
    [DisplayName("ProtocolStepCountPayload")]
    [Description("Creates a message payload that sets the number of steps to be executed in the current protocol.")]
    public partial class CreateProtocolStepCountPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the number of steps to be executed in the current protocol.
        /// </summary>
        [Range(min: 1, max: long.MaxValue)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the number of steps to be executed in the current protocol.")]
        public ushort ProtocolStepCount { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the ProtocolStepCount register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return ProtocolStepCount;
        }

        /// <summary>
        /// Creates a message that sets the number of steps to be executed in the current protocol.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ProtocolStepCount register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.ProtocolStepCount.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the number of steps to be executed in the current protocol.
    /// </summary>
    [DisplayName("TimestampedProtocolStepCountPayload")]
    [Description("Creates a timestamped message payload that sets the number of steps to be executed in the current protocol.")]
    public partial class CreateTimestampedProtocolStepCountPayload : CreateProtocolStepCountPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the number of steps to be executed in the current protocol.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ProtocolStepCount register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.ProtocolStepCount.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the period, in ms, of of each step in the protocol.
    /// </summary>
    [DisplayName("ProtocolPeriodPayload")]
    [Description("Creates a message payload that sets the period, in ms, of of each step in the protocol.")]
    public partial class CreateProtocolPeriodPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the period, in ms, of of each step in the protocol.
        /// </summary>
        [Range(min: 1, max: long.MaxValue)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the period, in ms, of of each step in the protocol.")]
        public ushort ProtocolPeriod { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the ProtocolPeriod register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return ProtocolPeriod;
        }

        /// <summary>
        /// Creates a message that sets the period, in ms, of of each step in the protocol.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ProtocolPeriod register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.ProtocolPeriod.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the period, in ms, of of each step in the protocol.
    /// </summary>
    [DisplayName("TimestampedProtocolPeriodPayload")]
    [Description("Creates a timestamped message payload that sets the period, in ms, of of each step in the protocol.")]
    public partial class CreateTimestampedProtocolPeriodPayload : CreateProtocolPeriodPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the period, in ms, of of each step in the protocol.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ProtocolPeriod register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.ProtocolPeriod.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies all the active events in the device.
    /// </summary>
    [DisplayName("EnableEventsPayload")]
    [Description("Creates a message payload that specifies all the active events in the device.")]
    public partial class CreateEnableEventsPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies all the active events in the device.
        /// </summary>
        [Description("The value that specifies all the active events in the device.")]
        public PumpEvents EnableEvents { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableEvents register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public PumpEvents GetPayload()
        {
            return EnableEvents;
        }

        /// <summary>
        /// Creates a message that specifies all the active events in the device.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableEvents register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.EnableEvents.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies all the active events in the device.
    /// </summary>
    [DisplayName("TimestampedEnableEventsPayload")]
    [Description("Creates a timestamped message payload that specifies all the active events in the device.")]
    public partial class CreateTimestampedEnableEventsPayload : CreateEnableEventsPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies all the active events in the device.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableEvents register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.EnableEvents.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that status of the protocol execution.
    /// </summary>
    [DisplayName("ProtocolPayload")]
    [Description("Creates a message payload that status of the protocol execution.")]
    public partial class CreateProtocolPayload
    {
        /// <summary>
        /// Gets or sets the value that status of the protocol execution.
        /// </summary>
        [Description("The value that status of the protocol execution.")]
        public ProtocolState Protocol { get; set; }

        /// <summary>
        /// Creates a message payload for the Protocol register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ProtocolState GetPayload()
        {
            return Protocol;
        }

        /// <summary>
        /// Creates a message that status of the protocol execution.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Protocol register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.Protocol.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that status of the protocol execution.
    /// </summary>
    [DisplayName("TimestampedProtocolPayload")]
    [Description("Creates a timestamped message payload that status of the protocol execution.")]
    public partial class CreateTimestampedProtocolPayload : CreateProtocolPayload
    {
        /// <summary>
        /// Creates a timestamped message that status of the protocol execution.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Protocol register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.Protocol.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the direction of the protocol execution.
    /// </summary>
    [DisplayName("ProtocolDirectionPayload")]
    [Description("Creates a message payload that sets the direction of the protocol execution.")]
    public partial class CreateProtocolDirectionPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the direction of the protocol execution.
        /// </summary>
        [Description("The value that sets the direction of the protocol execution.")]
        public ProtocolDirectionState ProtocolDirection { get; set; }

        /// <summary>
        /// Creates a message payload for the ProtocolDirection register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ProtocolDirectionState GetPayload()
        {
            return ProtocolDirection;
        }

        /// <summary>
        /// Creates a message that sets the direction of the protocol execution.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ProtocolDirection register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SyringePump.ProtocolDirection.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the direction of the protocol execution.
    /// </summary>
    [DisplayName("TimestampedProtocolDirectionPayload")]
    [Description("Creates a timestamped message payload that sets the direction of the protocol execution.")]
    public partial class CreateTimestampedProtocolDirectionPayload : CreateProtocolDirectionPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the direction of the protocol execution.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ProtocolDirection register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SyringePump.ProtocolDirection.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// The digital output lines.
    /// </summary>
    [Flags]
    public enum DigitalOutputs : byte
    {
        None = 0x0,
        DO0 = 0x1,
        DO1 = 0x2
    }

    /// <summary>
    /// The state of the digital input pin.
    /// </summary>
    [Flags]
    public enum DigitalInputs : byte
    {
        None = 0x0,
        DI0 = 0x1
    }

    /// <summary>
    /// The events that can be enabled/disabled.
    /// </summary>
    [Flags]
    public enum PumpEvents : byte
    {
        None = 0x0,
        Step = 0x1,
        Direction = 0x2,
        ForwardSwitch = 0x4,
        ReverseSwitch = 0x8,
        DigitalInput = 0x10,
        Protocol = 0x20
    }

    /// <summary>
    /// The state of the STEP motor controller pin.
    /// </summary>
    public enum StepState : byte
    {
        Low = 0,
        High = 1
    }

    /// <summary>
    /// The state of the DIR motor controller pin.
    /// </summary>
    public enum DirectionState : byte
    {
        Reverse = 0,
        Forward = 2
    }

    /// <summary>
    /// The state of the forward limit switch.
    /// </summary>
    public enum ForwardSwitchState : byte
    {
        Low = 0,
        High = 1
    }

    /// <summary>
    /// The state of the reverse limit switch.
    /// </summary>
    public enum ReverseSwitchState : byte
    {
        Low = 0,
        High = 2
    }

    /// <summary>
    /// Configures which signal is mimicked in the digital output 0.
    /// </summary>
    public enum DO0SyncConfig : byte
    {
        None = 0,
        SwitchState = 1
    }

    /// <summary>
    /// Configures which signal is mimicked in the digital output 1.
    /// </summary>
    public enum DO1SyncConfig : byte
    {
        None = 0,
        Heartbeat = 1,
        Step = 2
    }

    /// <summary>
    /// Configures the function executed when digital input is triggered.
    /// </summary>
    public enum DI0TriggerConfig : byte
    {
        None = 0,
        Step = 1,
        StartProtocol = 2
    }

    /// <summary>
    /// Available step modes.
    /// </summary>
    public enum StepModeType : byte
    {
        Full = 0,
        Half = 1,
        Quarter = 2,
        Eighth = 3,
        Sixteenth = 4
    }

    /// <summary>
    /// Available protocol types.
    /// </summary>
    public enum PumpProtocolType : byte
    {
        Step = 0,
        Volume = 1
    }

    /// <summary>
    /// Available board configurations.
    /// </summary>
    public enum PumpBoardType : byte
    {
        Pump = 0,
        FishFeeder = 1,
        StepperMotor = 2
    }

    /// <summary>
    /// The state of the protocol execution.
    /// </summary>
    public enum ProtocolState : byte
    {
        Idle = 0,
        Running = 1
    }

    /// <summary>
    /// The state of the protocol execution.
    /// </summary>
    public enum ProtocolDirectionState : byte
    {
        Reverse = 0,
        Forward = 1
    }
}

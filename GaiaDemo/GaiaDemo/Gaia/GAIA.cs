using System;

namespace GaiaDemo
{
    public static class GAIA
    {
        #region Header

        /// <summary>
        /// The mask which represents a command. 
        /// Mask used to retrieve the command from the packet.
        /// </summary>
        public const int COMMAND_MASK = 0x7FFF;

        /// <summary>
        /// The mask which represents an acknowledgement.
        /// COMMAND & ACKNOWLEDGMENT_MASK > 0, to know if the command is an acknowledgement.
        /// COMMAND | ACKNOWLEDGMENT_MASK, to build the acknowledgement command of a command.
        /// </summary>
        public const int ACKNOWLEDGMENT_MASK = 0x8000;

        /// <summary>
        /// The default value defined by the protocol for a "none" vendor.
        /// </summary>
        public const int VENDOR_NONE = 0x7FFE;

        /// <summary>
        /// The vendor default value defined by the protocol for Qualcomm vendor.
        /// </summary>
        public const int VENDOR_QUALCOMM = 0x000A;

        #endregion


        #region CONFIGURATION COMMANDS 0x01nn

        /// <summary>
        /// The mask to know if the command is a configuration command.
        /// </summary>
        public const int COMMANDS_CONFIGURATION_MASK = 0x0100;

        /// <summary>
        /// Retrieves the version of the configuration set.
        /// </summary>
        public const int COMMAND_GET_CONFIGURATION_VERSION = 0x0180;

        /// <summary>
        /// Configures the LED indicators. 
        /// Determines patterns to be displayed in given states and on events and configures filters to be applied as events occur.
        /// </summary>
        public const int COMMAND_SET_LED_CONFIGURATION = 0x0101;

        /// <summary>
        /// Retrieves the current LED configuration.
        /// </summary>
        public const int COMMAND_GET_LED_CONFIGURATION = 0x0181;

        /// <summary>
        /// Configures informational tones on the device.
        /// </summary>
        public const int COMMAND_SET_TONE_CONFIGURATION = 0x0102;

        /// <summary>
        /// Retrieves the currently configured tone configuration.
        /// </summary>
        public const int COMMAND_GET_TONE_CONFIGURATION = 0x0182;

        /// <summary>
        /// Sets the default volume for tones and audio.
        /// </summary>
        public const int COMMAND_SET_DEFAULT_VOLUME = 0x0103;

        /// <summary>
        /// Requests the default volume settings for tones and audio.
        /// </summary>
        public const int COMMAND_GET_DEFAULT_VOLUME = 0x0183;

        /// <summary>
        /// Resets all settings (deletes PS keys) which override factory defaults.
        /// </summary>
        public const int COMMAND_FACTORY_DEFAULT_RESET = 0x0104;

        /// <summary>
        /// Configures per-event vibrator patterns.
        /// </summary>
        public const int COMMAND_SET_VIBRATOR_CONFIGURATION = 0x0105;

        /// <summary>
        /// Retrieves the currently configured vibrator configuration.
        /// </summary>
        public const int COMMAND_GET_VIBRATOR_CONFIGURATION = 0x0185;

        /// <summary>
        /// Configures voice prompts to select a different language, voice etc.
        /// </summary>
        public const int COMMAND_SET_VOICE_PROMPT_CONFIGURATION = 0x0106;

        /// <summary>
        /// Retrieves the currently configured voice prompt configuration.
        /// </summary>
        public const int COMMAND_GET_VOICE_PROMPT_CONFIGURATION = 0x0186;

        /// <summary>
        /// Configures device features. 
        /// The feature identifiers are application dependent and would be documented with the application.
        /// </summary>
        public const int COMMAND_SET_FEATURE_CONFIGURATION = 0x0107;

        /// <summary>
        /// Retrieves settings of device features.
        /// </summary>
        public const int COMMAND_GET_FEATURE_CONFIGURATION = 0x0187;

        /// <summary>
        /// Set User Event Configuration.
        /// </summary>
        public const int COMMAND_SET_USER_EVENT_CONFIGURATION = 0x0108;

        /// <summary>
        /// Get User Event Configuration.
        /// </summary>
        public const int COMMAND_GET_USER_EVENT_CONFIGURATION = 0x0188;

        /// <summary>
        /// Configures the various timers on the device. 
        /// This command has a long form (where the payload holds the value of every timer) 
        /// and a short form(where the payload holds a timer number and the value of that timer).
        /// </summary>
        public const int COMMAND_SET_TIMER_CONFIGURATION = 0x0109;

        /// <summary>
        /// Retrieves the configuration of the various timers on the device. 
        /// This command has a long form (where the response holds the value of every timer) 
        /// and a short form(where the command payload holds a timer number and the response holds the number and value of that timer).
        /// </summary>
        public const int COMMAND_GET_TIMER_CONFIGURATION = 0x0189;

        /// <summary>
        /// Configures the device volume control for each of the 16 volume levels.
        /// </summary>
        public const int COMMAND_SET_AUDIO_GAIN_CONFIGURATION = 0x010A;

        /// <summary>
        /// Requests the device volume control configuration for each of the 16 volume levels.
        /// </summary>
        public const int COMMAND_GET_AUDIO_GAIN_CONFIGURATION = 0x018A;

        /// <summary>
        /// Set Volume Configuration.
        /// </summary>
        public const int COMMAND_SET_VOLUME_CONFIGURATION = 0x010B;

        /// <summary>
        /// Get Volume Configuration.
        /// </summary>
        public const int COMMAND_GET_VOLUME_CONFIGURATION = 0x018B;

        /// <summary>
        /// Set Power Configuration.
        /// </summary>
        public const int COMMAND_SET_POWER_CONFIGURATION = 0x010C;

        /// <summary>
        /// Get Power Configuration.
        /// </summary>
        public const int COMMAND_GET_POWER_CONFIGURATION = 0x018C;

        /// <summary>
        /// Set User Tone Configuration.
        /// </summary>
        public const int COMMAND_SET_USER_TONE_CONFIGURATION = 0x010E;

        /// <summary>
        /// Get User Tone Configuration.
        /// </summary>
        public const int COMMAND_GET_USER_TONE_CONFIGURATION = 0x018E;

        /// <summary>
        /// Set device name.
        /// </summary>
        public const int COMMAND_SET_DEVICE_NAME = 0x010F;

        /// <summary>
        /// Get device name.
        /// </summary>
        public const int COMMAND_GET_DEVICE_NAME = 0x018F;

        /// <summary>
        /// Sets the credentials to access the Wi-Fi access point.
        /// </summary>
        public const int COMMAND_SET_WLAN_CREDENTIALS = 0x0110;

        /// <summary>
        /// Retrieves the credentials to access the Wi-Fi access point.
        /// </summary>
        public const int COMMAND_GET_WLAN_CREDENTIALS = 0x0190;

        /// <summary>
        /// Sets peer permitted routing.
        /// </summary>
        public const int COMMAND_SET_PEER_PERMITTED_ROUTING = 0x0111;

        /// <summary>
        /// Gets peer permitted routing.
        /// </summary>
        public const int COMMAND_GET_PEER_PERMITTED_ROUTING = 0x0191;

        /// <summary>
        /// Sets permitted next audio source.
        /// </summary>
        public const int COMMAND_SET_PERMITTED_NEXT_AUDIO_SOURCE = 0x0112;

        /// <summary>
        /// Gets permitted next audio source.
        /// </summary>
        public const int COMMAND_GET_PERMITTED_NEXT_AUDIO_SOURCE = 0x0192;

        /// <summary>
        /// Sets the string to be sent to an AG to be dialled when the one-touch dialling feature is used.
        /// </summary>
        public const int COMMAND_SET_ONE_TOUCH_DIAL_STRING = 0x0116;

        /// <summary>
        /// Returns the string to be sent to an AG to be dialled when the one-touch dialling feature is used.
        /// </summary>
        public const int COMMAND_GET_ONE_TOUCH_DIAL_STRING = 0x0196;

        /// <summary>
        /// Gets Mounted partitions.
        /// </summary>
        public const int COMMAND_GET_MOUNTED_PARTITIONS = 0x01A0;

        /// <summary>
        /// Configures which SQIF partition is to be used for DFU operations.
        /// </summary>
        public const int COMMAND_SET_DFU_PARTITION = 0x0121;

        /// <summary>
        /// Retrieves the index and size of the configured DFU partition.
        /// </summary>
        public const int COMMAND_GET_DFU_PARTITION = 0x01A1;

        #endregion


        #region CONTROLS COMMANDS 0x02nn

        /// <summary>
        /// The mask to know if the command is a configuration command.
        /// </summary>
        public const int COMMANDS_CONTROLS_MASK = 0x0200;

        /// <summary>
        /// The host can raise/lower the current volume or mute/unmute audio using this command.
        /// </summary>
        public const int COMMAND_CHANGE_VOLUME = 0x0201;

        /// <summary>
        /// A host can cause a device to warm reset using this command. 
        /// The device will transmit an acknowledgement and then do a warm reset.
        /// </summary>
        public const int COMMAND_DEVICE_RESET = 0x0202;

        /// <summary>
        /// Requests the device's current boot mode.
        /// </summary>
        public const int COMMAND_GET_BOOT_MODE = 0x0282;

        /// <summary>
        /// Sets the state of device PIO pins.
        /// </summary>
        public const int COMMAND_SET_PIO_CONTROL = 0x0203;

        /// <summary>
        /// Gets the state of device PIOs.
        /// </summary>
        public const int COMMAND_GET_PIO_CONTROL = 0x0283;

        /// <summary>
        /// The host can request the device to physically power on or off by sending a SET_POWER_STATE command.
        /// The device will transmit an acknowledgement in response to the hosts request, if accepted the device shall also physically power on / off.
        /// </summary>
        public const int COMMAND_SET_POWER_STATE = 0x0204;

        /// <summary>
        /// The host can request to retrieve the devices current power state. 
        /// The device will transmit an acknowledgement and if successful, shall also indicate its current power state.
        /// </summary>
        public const int COMMAND_GET_POWER_STATE = 0x0284;

        /// <summary>
        /// Sets the orientation of the volume control buttons on the device.
        /// </summary>
        public const int COMMAND_SET_VOLUME_ORIENTATION = 0x0205;

        /// <summary>
        /// Requests the current orientation of the volume control buttons on the device.
        /// </summary>
        public const int COMMAND_GET_VOLUME_ORIENTATION = 0x0285;

        /// <summary>
        /// Enables or disables use of the vibrator in the headset, if one is present.
        /// </summary>
        public const int COMMAND_SET_VIBRATOR_CONTROL = 0x0206;

        /// <summary>
        /// Requests the current setting of the vibrator.
        /// </summary>
        public const int COMMAND_GET_VIBRATOR_CONTROL = 0x0286;

        /// <summary>
        /// Enables or disables LEDs (or equivalent indicators) on the headset.
        /// </summary>
        public const int COMMAND_SET_LED_CONTROL = 0x0207;

        /// <summary>
        /// Establishes whether LED indicators are enabled.
        /// </summary>
        public const int COMMAND_GET_LED_CONTROL = 0x0287;

        /// <summary>
        /// Sent from a headset to control an FM receiver on the phone, or from a handset to control a receiver in a headset.
        /// </summary>
        public const int COMMAND_FM_CONTROL = 0x0208;

        /// <summary>
        /// Play tone.
        /// </summary>
        public const int COMMAND_PLAY_TONE = 0x0209;

        /// <summary>
        /// Enables or disables voice prompts on the headset.
        /// </summary>
        public const int COMMAND_SET_VOICE_PROMPT_CONTROL = 0x020A;

        /// <summary>
        /// Establishes whether voice prompts are enabled.
        /// </summary>
        public const int COMMAND_GET_VOICE_PROMPT_CONTROL = 0x028A;

        /// <summary>
        /// Selects the next available language for Text-to-Speech functions.
        /// </summary>
        public const int COMMAND_CHANGE_AUDIO_PROMPT_LANGUAGE = 0x020B;

        /// <summary>
        /// Enables or disables simple speech recognition on the headset.
        /// </summary>
        public const int COMMAND_SET_SPEECH_RECOGNITION_CONTROL = 0x020C;

        /// <summary>
        /// Establishes whether speech recognition is enabled.
        /// </summary>
        public const int COMMAND_GET_SPEECH_RECOGNITION_CONTROL = 0x028C;

        /// <summary>
        /// Alert LEDs.
        /// </summary>
        public const int COMMAND_ALERT_LEDS = 0x020D;

        /// <summary>
        /// Alert tone.
        /// </summary>
        public const int COMMAND_ALERT_TONE = 0x020E;

        /// <summary>
        /// Alert the device user with LED patterns, tones or vibration. 
        /// The method and meaning of each alert is application-dependent and is configured using the appropriate LED, tone or vibrator event configuration.
        /// </summary>
        public const int COMMAND_ALERT_EVENT = 0x0210;

        /// <summary>
        /// Alert voice.
        /// </summary>
        public const int COMMAND_ALERT_VOICE = 0x0211;

        /// <summary>
        /// Sets audio prompt language.
        /// </summary>
        public const int COMMAND_SET_AUDIO_PROMPT_LANGUAGE = 0x0212;

        /// <summary>
        /// Gets audio prompt language.
        /// </summary>
        public const int COMMAND_GET_AUDIO_PROMPT_LANGUAGE = 0x0292;

        /// <summary>
        /// Starts the Simple Speech Recognition engine on the device. 
        /// A successful acknowledgement indicates that speech recognition has started; the actual speech recognition result will be relayed later via a GAIA_EVENT_SPEECH_RECOGNITION notification.
        /// </summary>
        public const int COMMAND_START_SPEECH_RECOGNITION = 0x0213;

        /// <summary>
        /// Selects an audio equaliser preset.
        /// </summary>
        public const int COMMAND_SET_EQ_CONTROL = 0x0214;

        /// <summary>
        /// Gets the currently selected audio equaliser preset.
        /// </summary>
        public const int COMMAND_GET_EQ_CONTROL = 0x0294;

        /// <summary>
        /// Enables or disables bass boost on the headset.
        /// </summary>
        public const int COMMAND_SET_BASS_BOOST_CONTROL = 0x0215;

        /// <summary>
        /// Establishes whether bass boost is enabled.
        /// </summary>
        public const int COMMAND_GET_BASS_BOOST_CONTROL = 0x0295;

        /// <summary>
        /// Enables or disables 3D sound enhancement on the headset.
        /// </summary>
        public const int COMMAND_SET_3D_ENHANCEMENT_CONTROL = 0x0216;

        /// <summary>
        /// Establishes whether 3D Enhancement is enabled.
        /// </summary>
        public const int COMMAND_GET_3D_ENHANCEMENT_CONTROL = 0x0296;

        /// <summary>
        /// Switches to the next available equaliser preset. 
        /// If issued while the last available preset is selected, switches to the first.
        /// </summary>
        public const int COMMAND_SWITCH_EQ_CONTROL = 0x0217;

        /// <summary>
        /// Turns on the Bass Boost effect if it was turned off; turns Bass Boost off if it was on.
        /// </summary>
        public const int COMMAND_TOGGLE_BASS_BOOST_CONTROL = 0x0218;

        /// <summary>
        /// Turns on the 3D Enhancement effect if it was turned off; turns 3D Enhancement off if it was on.
        /// </summary>
        public const int COMMAND_TOGGLE_3D_ENHANCEMENT_CONTROL = 0x0219;

        /// <summary>
        /// Sets a parameter of the parametric equaliser and optionally recalculates the filter coefficients.
        /// </summary>
        public const int COMMAND_SET_EQ_PARAMETER = 0x021A;

        /// <summary>
        /// Gets a parameter of the parametric equaliser.
        /// </summary>
        public const int COMMAND_GET_EQ_PARAMETER = 0x029A;

        /// <summary>
        /// Sets a group of parameters of the parametric equaliser.
        /// </summary>
        public const int COMMAND_SET_EQ_GROUP_PARAMETER = 0x021B;

        /// <summary>
        /// Gets a group of parameters of the parametric equaliser.
        /// </summary>
        public const int COMMAND_GET_EQ_GROUP_PARAMETER = 0x029B;

        /// <summary>
        /// Display control.
        /// </summary>
        public const int COMMAND_DISPLAY_CONTROL = 0x021C;

        /// <summary>
        /// Puts a Bluetooth device into pairing mode, making it discoverable and connectable.
        /// </summary>
        public const int COMMAND_ENTER_BLUETOOTH_PAIRING_MODE = 0x021D;

        /// <summary>
        /// Sets the device audio source.
        /// </summary>
        public const int COMMAND_SET_AUDIO_SOURCE = 0x021E;

        /// <summary>
        /// Gets the currently selected audio source.
        /// </summary>
        public const int COMMAND_GET_AUDIO_SOURCE = 0x029E;

        /// <summary>
        /// Sends an AVRC command to the device.
        /// </summary>
        public const int COMMAND_AV_REMOTE_CONTROL = 0x021F;

        /// <summary>
        /// Enables or disables the User-configured parametric equaliser on the device (compare Set EQ Control).
        /// </summary>
        public const int COMMAND_SET_USER_EQ_CONTROL = 0x0220;

        /// <summary>
        /// Establishes whether User EQ is enabled.
        /// </summary>
        public const int COMMAND_GET_USER_EQ_CONTROL = 0x02A0;

        /// <summary>
        /// Turns on the User EQ if it was turned off; turns User EQ off if it was on.
        /// </summary>
        public const int COMMAND_TOGGLE_USER_EQ_CONTROL = 0x0221;

        /// <summary>
        /// Enables or disables the speaker equaliser on the device.
        /// </summary>
        public const int COMMAND_SET_SPEAKER_EQ_CONTROL = 0x0222;

        /// <summary>
        /// Establishes whether Speaker EQ is enabled.
        /// </summary>
        public const int COMMAND_GET_SPEAKER_EQ_CONTROL = 0x02A2;

        /// <summary>
        /// Turns on the Speaker EQ if it was turned off; turns Speaker EQ off if it was on.
        /// </summary>
        public const int COMMAND_TOGGLE_SPEAKER_EQ_CONTROL = 0x0223;

        /// <summary>
        /// Controls the routing of True Wireless Stereo channels.
        /// </summary>
        public const int COMMAND_SET_TWS_AUDIO_ROUTING = 0x0224;

        /// <summary>
        /// Returns the current routing of True Wireless Stereo channels.
        /// </summary>
        public const int COMMAND_GET_TWS_AUDIO_ROUTING = 0x02A4;

        /// <summary>
        /// Controls the volume of True Wireless Stereo output.
        /// </summary>
        public const int COMMAND_SET_TWS_VOLUME = 0x0225;

        /// <summary>
        /// Returns the current volume setting of True Wireless Stereo.
        /// </summary>
        public const int COMMAND_GET_TWS_VOLUME = 0x02A5;

        /// <summary>
        /// Trims the volume of True Wireless Stereo output.
        /// </summary>
        public const int COMMAND_TRIM_TWS_VOLUME = 0x0226;

        /// <summary>
        /// Enables or disables reservation of one link for a peer device.
        /// </summary>
        public const int COMMAND_SET_PEER_LINK_RESERVED = 0x0227;

        /// <summary>
        /// Establishes whether one link is reserved for a peer device.
        /// </summary>
        public const int COMMAND_GET_PEER_LINK_RESERVED = 0x02A7;

        /// <summary>
        /// Requests the peer in a True Wireless Stereo session to begin Advertising. 
        /// The command payload length will be 1 if no target address is specified or 8 if a Typed Bluetooth Device Address is specified.
        /// </summary>
        public const int COMMAND_TWS_PEER_START_ADVERTISING = 0x022A;

        /// <summary>
        /// Requests the device send a "Find Me" request to the HID remote connected to it.
        /// </summary>
        public const int COMMAND_FIND_MY_REMOTE = 0x022B;

        /// <summary>
        /// Sets Codec.
        /// </summary>
        public const int COMMAND_SET_CODEC = 0x0240;

        /// <summary>
        /// Gets Codec.
        /// </summary>
        public const int COMMAND_GET_CODEC = 0x02C0;

        /// <summary>
        /// The command to set the supported features by Host for notification events. 
        /// Each feature corresponds to a mask as follow:
        /// 0x0001: time.
        /// 0x0002: missed calls.
        /// 0x0004: SMS.
        /// 0x0008: incoming call.
        /// </summary>
        public const int COMMAND_SET_SUPPORTED_FEATURES = 0x022C;

        /// <summary>
        /// The command to inform the Device that the Host will disconnect the GAIA connection from the Device.
        /// </summary>
        public const int COMMAND_DISCONNECT = 0x022D;

        /// <summary>
        /// Sets the transfer mode to use for the Data Endpoint. 
        /// Known transfer modes are:
        /// 0x00: disable Data Endpoint.
        /// 0x01: RWCP transfer mode - Reliable Write Command Protocol.
        /// </summary>
        public const int COMMAND_SET_DATA_ENDPOINT_MODE = 0x022E;

        /// <summary>
        /// Gets the transfer mode currently set for the Data Endpoint.
        /// Known transfer modes are:
        /// 0x00: disable Data Endpoint.
        /// 0x01: RWCP transfer mode - Reliable Write Command Protocol.
        /// </summary>
        public const int COMMAND_GET_DATA_ENDPOINT_MODE = 0x02AE;

        #endregion


        #region POLLED STATUS COMMANDS 0x03nn

        /// <summary>
        /// The mask to know if the command is a polled status command.
        /// </summary>
        public const int COMMANDS_POLLED_STATUS_MASK = 0x0300;

        /// <summary>
        /// Gets the Gaia Protocol and API version numbers from the device.
        /// </summary>
        public const int COMMAND_GET_API_VERSION = 0x0300;

        /// <summary>
        /// Gets the current RSSI value for the Bluetooth link from the device. 
        /// The RSSI is specified in dBm using 2's compliment representation, e.g. -20 = 0xEC
        /// </summary>
        public const int COMMAND_GET_CURRENT_RSSI = 0x0301;

        /// <summary>
        /// Gets the current battery level from the device. 
        /// Battery level is specified in mV stored as a uint16, e.g. 3,300mV = 0x0CE4.
        /// </summary>
        public const int COMMAND_GET_CURRENT_BATTERY_LEVEL = 0x0302;

        /// <summary>
        /// Requests the BlueCore hardware, design and module identification.
        /// </summary>
        public const int COMMAND_GET_MODULE_ID = 0x0303;

        /// <summary>
        /// Requests the application software to identify itself. 
        /// The acknowledgement payload contains eight octets of application identification optionally followed by nul-terminated human-readable text.
        /// The identification information is application dependent; the headset copies fields from the Bluetooth Device ID.
        /// </summary>
        public const int COMMAND_GET_APPLICATION_VERSION = 0x0304;

        /// <summary>
        /// Requests the logic state of the chip PIOs.
        /// </summary>
        public const int COMMAND_GET_PIO_STATE = 0x0306;

        /// <summary>
        /// Requests the value read by a given analogue-to-digital converter.
        /// </summary>
        public const int COMMAND_READ_ADC = 0x0307;

        /// <summary>
        /// Requests the Bluetooth device address of the peer.
        /// </summary>
        public const int COMMAND_GET_PEER_ADDRESS = 0x030A;

        /// <summary>
        /// To request status of certain information from the host. 
        /// Here we are talking about information as system notifications such an incoming SMS, a missed call information, etc.
        /// </summary>
        public const int COMMAND_GET_HOST_FEATURE_INFORMATION = 0x0320;

        #endregion


        #region FEATURE CONTROL COMMANDS 0x05nn

        /// <summary>
        /// The mask to know if the command is a polled status command.
        /// </summary>
        public const int COMMANDS_FEATURE_CONTROL_MASK = 0x0500;

        /// <summary>
        /// Gets Authentication bitmaps.
        /// </summary>
        public const int COMMAND_GET_AUTH_BITMAPS = 0x0580;

        /// <summary>
        /// Initiates a Gaia Authentication exchange.
        /// </summary>
        public const int COMMAND_AUTHENTICATE_REQUEST = 0x0501;

        /// <summary>
        /// Provides authentication credentials.
        /// </summary>
        public const int COMMAND_AUTHENTICATE_RESPONSE = 0x0502;

        /// <summary>
        /// The host can use this command to enable or disable a feature which it is authenticated to use.
        /// </summary>
        public const int COMMAND_SET_FEATURE = 0x0503;

        /// <summary>
        /// The host can use this command to request the status of a feature.
        /// </summary>
        public const int COMMAND_GET_FEATURE = 0x0583;

        /// <summary>
        /// The host uses this command to enable a GAIA session with a device which does not have the session enabled by default.
        /// </summary>
        public const int COMMAND_SET_SESSION_ENABLE = 0x0504;

        /// <summary>
        /// Retrieves the session enabled state.
        /// </summary>
        public const int COMMAND_GET_SESSION_ENABLE = 0x0584;

        #endregion


        #region DATA TRANSFER COMMANDS 0x06nn

        /// <summary>
        /// The mask to know if the command is a data transfer command.
        /// </summary>
        public const int COMMANDS_DATA_TRANSFER_MASK = 0x0600;

        /// <summary>
        /// Initialises a data transfer session.
        /// </summary>
        public const int COMMAND_DATA_TRANSFER_SETUP = 0x0601;

        /// <summary>
        /// The host uses this command to indicate closure of a data transfer session, providing the Session ID in the packet payload.
        /// The device can release any resources required to maintain a data transfer session at this point, as the host must perform another Data Transfer Setup before sending any more data.
        /// </summary>
        public const int COMMAND_DATA_TRANSFER_CLOSE = 0x0602;

        /// <summary>
        /// A host can use this command to transfer data to a device.
        /// </summary>
        public const int COMMAND_HOST_TO_DEVICE_DATA = 0x0603;

        /// <summary>
        /// A device can use this command to transfer data to the host.
        /// </summary>
        public const int COMMAND_DEVICE_TO_HOST_DATA = 0x0604;

        /// <summary>
        /// Initiates an I2C Transfer (write and/or read).
        /// </summary>
        public const int COMMAND_I2C_TRANSFER = 0x0608;

        /// <summary>
        /// Retrieves information on a storage partition.
        /// </summary>
        public const int COMMAND_GET_STORAGE_PARTITION_STATUS = 0x0610;

        /// <summary>
        /// Prepares a device storage partition for access from the host.
        /// </summary>
        public const int COMMAND_OPEN_STORAGE_PARTITION = 0x0611;

        /// <summary>
        /// Prepares a UART for access from the host.
        /// </summary>
        public const int COMMAND_OPEN_UART = 0x0612;

        /// <summary>
        /// Writes raw data to an open storage partition.
        /// </summary>
        public const int COMMAND_WRITE_STORAGE_PARTITION = 0x0615;

        /// <summary>
        /// Writes data to an open stream.
        /// </summary>
        public const int COMMAND_WRITE_STREAM = 0x0617;

        /// <summary>
        /// Closes a storage partition.
        /// </summary>
        public const int COMMAND_CLOSE_STORAGE_PARTITION = 0x0618;

        /// <summary>
        /// Mounts a device storage partition for access from the device.
        /// </summary>
        public const int COMMAND_MOUNT_STORAGE_PARTITION = 0x061A;

        /// <summary>
        /// Gets file status.
        /// </summary>
        public const int COMMAND_GET_FILE_STATUS = 0x0620;

        /// <summary>
        /// Prepares a file for access from the host.
        /// </summary>
        public const int COMMAND_OPEN_FILE = 0x0621;

        /// <summary>
        /// Reads data from an open file.
        /// </summary>
        public const int COMMAND_READ_FILE = 0x0624;

        /// <summary>
        /// Closes a file.
        /// </summary>
        public const int COMMAND_CLOSE_FILE = 0x0628;

        /// <summary>
        /// Indicates to the host that the device wishes to receive a Device Firmware Upgrade image.
        /// </summary>
        public const int COMMAND_DFU_REQUEST = 0x0630;

        /// <summary>
        /// Readies the device to receive a Device Firmware Upgrade image. 
        /// The payload will be 8 or 136 octets depending on the message digest type.
        /// </summary>
        public const int COMMAND_DFU_BEGIN = 0x0631;

        /// <summary>
        /// DFU write.
        /// </summary>
        public const int COMMAND_DFU_WRITE = 0x0632;

        /// <summary>
        /// Commands the device to install the DFU image and restart.
        /// </summary>
        public const int COMMAND_DFU_COMMIT = 0x0633;

        /// <summary>
        /// Requests the status of the last completed DFU operation.
        /// </summary>
        public const int COMMAND_DFU_GET_RESULT = 0x0634;

        /// <summary>
        /// Begins a VM Upgrade session over GAIA, allowing VM Upgrade Protocol packets to be sent using the VM Upgrade Control and VM Upgrade Data commands.
        /// </summary>
        public const int COMMAND_VM_UPGRADE_CONNECT = 0x0640;

        /// <summary>
        /// Ends a VM Upgrade session over GAIA.
        /// </summary>
        public const int COMMAND_VM_UPGRADE_DISCONNECT = 0x0641;

        /// <summary>
        /// Tunnels a VM Upgrade Protocol packet.
        /// </summary>
        public const int COMMAND_VM_UPGRADE_CONTROL = 0x0642;

        /// <summary>
        /// Introduces VM Upgrade Protocol data.
        /// </summary>
        public const int COMMAND_VM_UPGRADE_DATA = 0x0643;

        #endregion


        #region DEBUGGING COMMANDS 0x07nn

        /// <summary>
        /// The mask to know if the command is a polled status command.
        /// </summary>
        public const int COMMANDS_DEBUGGING_MASK = 0x0700;

        /// <summary>
        /// Requests the device to perform no operation; serves to establish that the Gaia protocol handler is alive.
        /// </summary>
        public const int COMMAND_NO_OPERATION = 0x0700;

        /// <summary>
        /// Requests the values of the device debugging flags.
        /// </summary>
        public const int COMMAND_GET_DEBUG_FLAGS = 0x0701;

        /// <summary>
        /// Sets the values of the device debugging flags.
        /// </summary>
        public const int COMMAND_SET_DEBUG_FLAGS = 0x0702;

        /// <summary>
        /// Retrieves the value of the indicated PS key.
        /// </summary>
        public const int COMMAND_RETRIEVE_PS_KEY = 0x0710;

        /// <summary>
        /// Retrieves the value of the indicated PS key.
        /// </summary>
        public const int COMMAND_RETRIEVE_FULL_PS_KEY = 0x0711;

        /// <summary>
        /// Sets the value of the indicated PS key.
        /// </summary>
        public const int COMMAND_STORE_PS_KEY = 0x0712;

        /// <summary>
        /// Flood fill the store to force a defragment at next boot.
        /// </summary>
        public const int COMMAND_FLOOD_PS = 0x0713;

        /// <summary>
        /// Sets the value of the indicated PS key.
        /// </summary>
        public const int COMMAND_STORE_FULL_PS_KEY = 0x0714;

        /// <summary>
        /// Results in a GAIA_DEBUG_MESSAGE being sent up from the Gaia library to the application task. 
        /// Its interpretation is entirely user defined.
        /// </summary>
        public const int COMMAND_SEND_DEBUG_MESSAGE = 0x0720;

        /// <summary>
        /// Sends an arbitrary message to the on-chip application.
        /// </summary>
        public const int COMMAND_SEND_APPLICATION_MESSAGE = 0x0721;

        /// <summary>
        /// Sends an arbitrary message to the Kalimba DSP.
        /// </summary>
        public const int COMMAND_SEND_KALIMBA_MESSAGE = 0x0722;

        /// <summary>
        /// Retrieves the number of available malloc() slots and the space available for PS keys.
        /// </summary>
        public const int COMMAND_GET_MEMORY_SLOTS = 0x0730;

        /// <summary>
        /// Retrieves the value of the specified 16-bit debug variable.
        /// </summary>
        public const int COMMAND_GET_DEBUG_VARIABLE = 0x0740;

        /// <summary>
        /// Sets the value of the specified 16-bit debug variable.
        /// </summary>
        public const int COMMAND_SET_DEBUG_VARIABLE = 0x0741;

        /// <summary>
        /// Removes all authenticated devices from the paired device list and any associated attribute data.
        /// </summary>
        public const int COMMAND_DELETE_PDL = 0x0750;

        /// <summary>
        /// Sent to a BLE slave device, causing it to request a new set of connection parameters.
        /// </summary>
        public const int COMMAND_SET_BLE_CONNECTION_PARAMETERS = 0x0752;

        #endregion


        #region IVOR COMMANDS 0x10nn

        /// <summary>
        /// The mask to know if the command is an ivor status command.
        /// </summary>
        public const int COMMANDS_IVOR_MASK = 0x1000;

        /// <summary>
        /// Used by a device to request the start of a voice assistant session.
        /// </summary>
        public const int COMMAND_IVOR_START = 0x1000;

        /// <summary>
        /// Used by the host to request the device to stream the voice request of a voice assistant session.
        /// </summary>
        public const int COMMAND_IVOR_VOICE_DATA_REQUEST = 0x1001;

        /// <summary>
        /// Used by the device to stream the voice.
        /// Warning: no acknowledgement is sent for this command.
        /// </summary>
        public const int COMMAND_IVOR_VOICE_DATA = 0x1002;

        /// <summary>
        /// Ued by the host to indicate the device to stop to stream the voice.
        /// </summary>
        public const int COMMAND_IVOR_VOICE_END = 0x1003;

        /// <summary>
        /// Used by the host or the device to cancel a voice assistant session.
        /// </summary>
        public const int COMMAND_IVOR_CANCEL = 0x1004;

        /// <summary>
        /// Used by the device to check if the host supports its IVOR version.
        /// </summary>
        public const int COMMAND_IVOR_CHECK_VERSION = 0x1005;

        /// <summary>
        /// Used by the host to indicate to the device that the voice assistant response is going to be played.
        /// </summary>
        public const int COMMAND_IVOR_ANSWER_START = 0x1006;

        /// <summary>
        /// Used by the host to indicate to the device that the voice assistant response has finished to be played.
        /// </summary>
        public const int COMMAND_IVOR_ANSWER_END = 0x1007;


        public const int COMMAND_IVOR_PING = 0x10F0;

        #endregion


        #region NOTIFICATION COMMANDS 0x40nn

        /// <summary>
        /// The mask to know if the command is a notification command.
        /// </summary>
        public const int COMMANDS_NOTIFICATION_MASK = 0x4000;

        /// <summary>
        /// Hosts register for notifications using the REGISTER_NOTIFICATION command, 
        /// specifying an Event Type from table below as the first byte of payload, 
        /// with optional parameters as defined per event in successive payload bytes.
        /// </summary>
        public const int COMMAND_REGISTER_NOTIFICATION = 0x4001;

        /// <summary>
        /// Requests the current status of an event type. 
        /// For threshold type events where multiple levels may be registered, the response indicates how many notifications are registered.
        /// Where an event may be simply registered or not the number will be 1 or 0.
        /// </summary>
        public const int COMMAND_GET_NOTIFICATION = 0x4081;

        /// <summary>
        /// A host can cancel event notification by sending a CANCEL_NOTIFICATION command, 
        /// the first byte of payload will be the Event Type being cancelled.
        /// </summary>
        public const int COMMAND_CANCEL_NOTIFICATION = 0x4002;

        /// <summary>
        /// Assuming successful registration, the host will asynchronously receive one or more EVENT_NOTIFICATION command(s) (Command ID 0x4003). 
        /// The first byte of the Event Notification command payload will be the Event Type code, indicating the notification type.
        /// For example, 0x03 indicating a battery level low threshold event notification.
        /// Further data in the Event Notification payload is dependent on the notification type and defined on a per-notification basis below.
        /// </summary>
        public const int COMMAND_EVENT_NOTIFICATION = 0x4003;

        #endregion


        #region COMMAND STATUSES

        /// <summary>
        /// The different status for an acknowledgment packet.
        /// By convention, the first octet in an acknowledgement (ACK) packet is a status code indicating the success or the reason for the failure of a request.
        /// </summary>
        public enum Status
        {
            NOT_STATUS = -1,
            /// <summary>
            /// The request completed successfully.
            /// </summary>
            SUCCESS = 0,
            /// <summary>
            /// An invalid COMMAND ID has been sent or is not supported by the device.
            /// </summary>
            NOT_SUPPORTED = 1,
            /// <summary>
            /// The host is not authenticated to use a Command ID or to control a feature type.
            /// </summary>
            NOT_AUTHENTICATED = 2,
            /// <summary>
            /// The COMMAND ID used is valid but the GAIA device could not complete it successfully.
            /// </summary>
            INSUFFICIENT_RESOURCES = 3,
            /// <summary>
            /// The GAIA device is in the process of authenticating the host.
            /// </summary>
            AUTHENTICATING = 4,
            /// <summary>
            /// The parameters sent were invalid: missing parameters, too much parameters, range, etc.
            /// </summary>
            INVALID_PARAMETER = 5,
            /// <summary>
            /// The GAIA device is not in the correct state to process the command: needs to stream music, use a certain source, etc.
            /// </summary>
            INCORRECT_STATE = 6,
            /// <summary>
            /// The command is in progress.
            /// Acknowledgements with IN_PROGRESS status may be sent once 
            /// or periodically during the processing of a time-consuming operation to indicate that the operation has not stalled.
            /// </summary>
            IN_PROGRESS = 7
        }

        /// <summary>
        /// To identify a GAIA.Status by its value.
        /// </summary>
        /// <param name="status">The status to identify.</param>
        /// <returns>the GAIA.Status</returns>
        public static Status GetStatus(byte status)
        {
            switch (status)
            {
                case 0x00:
                    return Status.SUCCESS;
                case 0x01:
                    return Status.NOT_SUPPORTED;
                case 0x02:
                    return Status.NOT_AUTHENTICATED;
                case 0x03:
                    return Status.INSUFFICIENT_RESOURCES;
                case 0x04:
                    return Status.AUTHENTICATING;
                case 0x05:
                    return Status.INVALID_PARAMETER;
                case 0x06:
                    return Status.INCORRECT_STATE;
                case 0x07:
                    return Status.IN_PROGRESS;
                default:
                    return Status.NOT_STATUS;
            }
        }

        /// <summary>
        /// To obtain a readable version of a GAIA.Status.
        /// </summary>
        /// <param name="status">the status value.</param>
        /// <returns>A string corresponding to the GAIA.Status value.</returns>
        public static string GetStatusToString(int status)
        {
            switch (status)
            {
                case 0:
                    return "SUCCESS";
                case 1:
                    return "NOT SUPPORTED";
                case 2:
                    return "NOT AUTHENTICATED";
                case 3:
                    return "INSUFFICIENT RESOURCES";
                case 4:
                    return "AUTHENTICATING";
                case 5:
                    return "INVALID PARAMETER";
                case 6:
                    return "INCORRECT STATE";
                case 7:
                    return "IN PROGRESS";
                case -1:
                    return "NOT STATUS";
                default:
                    return "UNKNOWN STATUS";
            }
        }

        #endregion


        #region NOTIFICATION EVENTS

        /// <summary>
        /// All notification event types which can be sent by the device.
        /// </summary>
        public enum NotificationEvents
        {
            /// <summary>
            /// This is not a notification - 0x00
            /// </summary>
            NOT_NOTIFICATION = 0,
            /// <summary>
            /// This event provides a way for hosts to receive notification of changes in the RSSI of a device's Bluetooth link with the host - 0x01
            /// </summary>
            RSSI_LOW_THRESHOLD = 0x01,
            /// <summary>
            /// This command provides a way for hosts to receive notification of changes in the RSSI of a device's Bluetooth link with the host - 0x02
            /// </summary>
            RSSI_HIGH_THRESHOLD = 0x02,
            /// <summary>
            /// This command provides a way for hosts to receive notification of changes in the battery level of a device - 0x03
            /// </summary>
            BATTERY_LOW_THRESHOLD = 0x03,
            /// <summary>
            /// This command provides a way for hosts to receive notification of changes in the battery level of a device - 0x04
            /// </summary>
            BATTERY_HIGH_THRESHOLD = 0x04,
            /// <summary>
            /// A host can register to receive notifications of the device changes in state - 0x05
            /// </summary>
            DEVICE_STATE_CHANGED = 0x05,
            /// <summary>
            /// A host can register to receive notification of a change in PIO state. 
            /// The host provides a uint32 bitmap of PIO pins about which it wishes to receive state change notifications - 0x06
            /// </summary>
            PIO_CHANGED = 0x06,
            /// <summary>
            /// A host can register to receive debug messages from a device - 0x07
            /// </summary>
            DEBUG_MESSAGE = 0x07,
            /// <summary>
            /// A host can register to receive a notification when the device battery has been fully charged - 0x08
            /// </summary>
            BATTERY_CHARGED = 0x08,
            /// <summary>
            /// A host can register to receive a notification when the battery charger is connected or disconnected - 0x09
            /// </summary>
            CHARGER_CONNECTION = 0x09,
            /// <summary>
            /// A host can register to receive a notification when the capacitive touch sensors' state changes. 
            /// Removed from V1.0 of the API but sounds useful - 0x0A
            /// </summary>
            CAPSENSE_UPDATE = 0x0A,
            /// <summary>
            /// A host can register to receive a notification when an application-specific user action takes place, 
            /// for instance a long button press.Not the same as PIO Changed.
            /// Removed from V1.0 of the API but sounds useful - 0x0B
            /// </summary>
            USER_ACTION = 0x0B,
            /// <summary>
            /// A host can register to receive a notification when the Speech Recognition system thinks it heard something - 0x0C
            /// </summary>
            SPEECH_RECOGNITION = 0x0C,
            /// <summary>
            /// ? - 0x0D
            /// </summary>
            AV_COMMAND = 0x0D,
            /// <summary>
            /// ? - 0x0E
            /// </summary>
            REMOTE_BATTERY_LEVEL = 0x0E,
            /// <summary>
            /// ? - 0x0F
            /// </summary>
            KEY = 0x0F,
            /// <summary>
            /// This notification event indicates the progress of a Device Firmware Upgrade operation - 0x10
            /// </summary>
            DFU_STATE = 0x10,
            /// <summary>
            /// This notification event indicates that data has been received by a UART - 0x11
            /// </summary>
            UART_RECEIVED_DATA = 0x11,
            /// <summary>
            /// This notification event encapsulates a VM Upgrade Protocol packet - 0x12
            /// </summary>
            VMU_PACKET = 0x12,
            /// <summary>
            /// This notification event encapsulates all system notification from the Host, such has an incoming call.
            /// </summary>
            HOST_NOTIFICATION = 0x13
        }

        /// <summary>
        /// To identify a GAIA.NotificationEvents by its value.
        /// </summary>
        /// <param name="notificationEvent">The event to identify.</param>
        /// <returns>the GAIA.NotificationEvents</returns>
        public static NotificationEvents GetNotificationEvent(byte notificationEvent)
        {
            switch (notificationEvent)
            {
                case 0x01:
                    return NotificationEvents.RSSI_LOW_THRESHOLD;
                case 0x02:
                    return NotificationEvents.RSSI_HIGH_THRESHOLD;
                case 0x03:
                    return NotificationEvents.BATTERY_LOW_THRESHOLD;
                case 0x04:
                    return NotificationEvents.BATTERY_HIGH_THRESHOLD;
                case 0x05:
                    return NotificationEvents.DEVICE_STATE_CHANGED;
                case 0x06:
                    return NotificationEvents.PIO_CHANGED;
                case 0x07:
                    return NotificationEvents.DEBUG_MESSAGE;
                case 0x08:
                    return NotificationEvents.BATTERY_CHARGED;
                case 0x09:
                    return NotificationEvents.CHARGER_CONNECTION;
                case 0x0A:
                    return NotificationEvents.CAPSENSE_UPDATE;
                case 0x0B:
                    return NotificationEvents.USER_ACTION;
                case 0x0C:
                    return NotificationEvents.SPEECH_RECOGNITION;
                case 0x0D:
                    return NotificationEvents.AV_COMMAND;
                case 0x0E:
                    return NotificationEvents.REMOTE_BATTERY_LEVEL;
                case 0x0F:
                    return NotificationEvents.KEY;
                case 0x10:
                    return NotificationEvents.DFU_STATE;
                case 0x11:
                    return NotificationEvents.UART_RECEIVED_DATA;
                case 0x12:
                    return NotificationEvents.VMU_PACKET;
                case 0x13:
                    return NotificationEvents.HOST_NOTIFICATION;
                default:
                    return NotificationEvents.NOT_NOTIFICATION;
            }
        }

        #endregion


        #region TRANSPORT

        /// <summary>
        /// All known transports which can be used to send and transfer GAIA packets as their format changes depending on their transport.
        /// </summary>
        public enum Transport
        {
            /// <summary>
            /// Bluetooth Low Energy.
            /// </summary>
            BLE = 0,
            /// <summary>
            /// Bluetooth Classic.
            /// </summary>
            BR_EDR = 1
        }

        #endregion

        #region deprecated

        public const int COMMAND_SET_RAW_CONFIGURATION = 0x0100;
        public const int COMMAND_GET_CONFIGURATION_ID = 0x0184;
        public const int COMMAND_GET_DFU_STATUS = 0x0310;

        #endregion
    }
}

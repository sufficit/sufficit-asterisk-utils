# Usage Examples - Sufficit.Asterisk.Utils

## Getting Started

### Prerequisites
* **.NET SDK** - Version depends on your target framework
* **Basic understanding** of Asterisk concepts (channels, extensions, etc.)

## Channel Utilities

### Channel Information Parsing

```csharp
using Sufficit.Asterisk.Utils;

// Parse channel information
var channelInfo = ChannelUtils.ParseChannel("SIP/1000-00000001");
Console.WriteLine($"Technology: {channelInfo.Technology}"); // SIP
Console.WriteLine($"Peer: {channelInfo.Peer}");            // 1000
Console.WriteLine($"UniqueId: {channelInfo.UniqueId}");    // 00000001

// Generate channel names
var newChannel = ChannelUtils.GenerateChannel("PJSIP", "extension123");
Console.WriteLine(newChannel); // PJSIP/extension123-{unique-id}

// Validate channel format
bool isValid = ChannelUtils.IsValidChannel("SIP/1000-00000001");
```

## Number Formatting

### Phone Number Operations

```csharp
using Sufficit.Asterisk.Utils;

// Format phone numbers for display
var formatted = NumberUtils.FormatForDisplay("+5511999887766");
Console.WriteLine(formatted); // +55 (11) 99988-7766

// Normalize for dialing
var normalized = NumberUtils.NormalizeForDialing("(11) 99988-7766");
Console.WriteLine(normalized); // 1199988766

// Validate phone numbers
bool isValid = NumberUtils.IsValidPhoneNumber("+5511999887766");

// Extract area code
var areaCode = NumberUtils.ExtractAreaCode("1199988766");
Console.WriteLine(areaCode); // 11
```

## Call Duration Utilities

### Duration Calculations

```csharp
using Sufficit.Asterisk.Utils;

// Calculate billing seconds from timestamps
var startTime = DateTime.Parse("2024-01-15 10:30:00");
var answerTime = DateTime.Parse("2024-01-15 10:30:05");
var endTime = DateTime.Parse("2024-01-15 10:35:30");

var billSec = CallDurationUtils.CalculateBillSec(answerTime, endTime);
Console.WriteLine($"Billable seconds: {billSec}"); // 325

var duration = CallDurationUtils.CalculateDuration(startTime, endTime);
Console.WriteLine($"Total duration: {duration}"); // 00:05:30

// Format duration for display
var displayDuration = CallDurationUtils.FormatDuration(325);
Console.WriteLine(displayDuration); // 05:25

// Calculate cost based on duration and rate
var cost = CallDurationUtils.CalculateCost(325, 0.05m); // 325 seconds at $0.05/minute
Console.WriteLine($"Call cost: ${cost:F2}"); // $0.27
```

## Extension Pattern Matching

### Pattern Validation

```csharp
using Sufficit.Asterisk.Utils;

// Match extension patterns
bool matches = ExtensionUtils.MatchesPattern("1234", "1XXX");
Console.WriteLine(matches); // true

matches = ExtensionUtils.MatchesPattern("5678", "1XXX");  
Console.WriteLine(matches); // false

// Generate regex from Asterisk pattern
var regex = ExtensionUtils.PatternToRegex("_1NXXXXXX");
Console.WriteLine(regex.IsMatch("18005551234")); // true

// Validate extension format
bool isValidExt = ExtensionUtils.IsValidExtension("1001");
```

## CDR Processing

### Call Detail Record Operations

```csharp
using Sufficit.Asterisk.Utils;

// Parse CDR record
var cdrLine = "\"1001\",\"1002\",\"2024-01-15 10:30:00\",\"2024-01-15 10:30:05\",\"2024-01-15 10:35:30\",325,\"ANSWERED\"";
var cdr = CDRUtils.ParseCDRLine(cdrLine);

Console.WriteLine($"Source: {cdr.Source}");
Console.WriteLine($"Destination: {cdr.Destination}");
Console.WriteLine($"Duration: {cdr.BillSec} seconds");
Console.WriteLine($"Status: {cdr.Disposition}");

// Calculate statistics from CDR collection
var cdrList = GetCDRList(); // Your CDR collection
var stats = CDRUtils.CalculateStatistics(cdrList);

Console.WriteLine($"Total calls: {stats.TotalCalls}");
Console.WriteLine($"Answered calls: {stats.AnsweredCalls}");
Console.WriteLine($"Answer rate: {stats.AnswerRate:P1}");
Console.WriteLine($"Average duration: {stats.AverageDuration}");
```

## Configuration Generators

### Asterisk Configuration Creation

```csharp
using Sufficit.Asterisk.Utils;

// Generate SIP peer configuration
var sipConfig = ConfigUtils.GenerateSIPPeer(new SIPPeerConfig
{
    Name = "1001",
    Secret = "secretpassword",
    Host = "dynamic",
    Context = "internal",
    Qualify = true,
    CanReinvite = false
});

Console.WriteLine(sipConfig);
// Output:
// [1001]
// secret=secretpassword
// host=dynamic
// context=internal
// qualify=yes
// canreinvite=no

// Generate dial plan extension
var extension = ConfigUtils.GenerateExtension(new ExtensionConfig
{
    Context = "internal",
    Extension = "1001",
    Priority = 1,
    Application = "Dial",
    Parameters = "SIP/1001,30"
});

Console.WriteLine(extension);
// Output: exten => 1001,1,Dial(SIP/1001,30)
```

## Queue Management Utilities

### Queue Operations

```csharp
using Sufficit.Asterisk.Utils;

// Parse queue member status
var memberInfo = QueueUtils.ParseMemberStatus("SIP/1001 (ringinuse disabled) (realtime) (In use) has taken 5 calls (last was 120 secs ago)");
Console.WriteLine($"Interface: {memberInfo.Interface}"); // SIP/1001
Console.WriteLine($"Status: {memberInfo.Status}");       // In use
Console.WriteLine($"Calls taken: {memberInfo.CallsTaken}"); // 5

// Calculate queue statistics
var queueStats = QueueUtils.CalculateQueueStats(queueMembers, queueCalls);
Console.WriteLine($"Available agents: {queueStats.AvailableAgents}");
Console.WriteLine($"Calls waiting: {queueStats.WaitingCalls}");
Console.WriteLine($"Average wait time: {queueStats.AverageWaitTime}");
```

## Audio Format Utilities

### Codec and Format Operations

```csharp
using Sufficit.Asterisk.Utils;

// Convert between audio formats
var formats = AudioUtils.ParseFormats("alaw,ulaw,gsm");
Console.WriteLine(string.Join(", ", formats)); // alaw, ulaw, gsm

// Get codec information
var codecInfo = AudioUtils.GetCodecInfo("g729");
Console.WriteLine($"Bitrate: {codecInfo.Bitrate}"); // 8000
Console.WriteLine($"Frame size: {codecInfo.FrameSize}"); // 20ms

// Validate codec compatibility
bool compatible = AudioUtils.AreCodecsCompatible("alaw", "ulaw");
Console.WriteLine(compatible); // true (both are G.711)
```

## Extension Methods

### Enhanced Type Functionality

```csharp
using Sufficit.Asterisk.Utils;

// String extensions for telephony
string phoneNumber = "+5511999887766";
bool isPhone = phoneNumber.IsPhoneNumber();
string normalized = phoneNumber.NormalizePhoneNumber();

// DateTime extensions for call records
DateTime callTime = DateTime.Now;
long unixTimestamp = callTime.ToUnixTimestamp();
DateTime fromUnix = unixTimestamp.FromUnixTimestamp();

// Channel string extensions
string channel = "SIP/1000-00000001";
string technology = channel.GetChannelTechnology(); // SIP
string peer = channel.GetChannelPeer();             // 1000
bool isActive = channel.IsActiveChannel();
```

## Performance Features

### High-Performance Operations

```csharp
// High-performance number parsing with minimal allocations
var results = new List<string>();
for (int i = 0; i < 100000; i++)
{
    // Optimized parsing reuses string builders and regex instances
    var normalized = NumberUtils.NormalizeForDialing($"({i % 100:D2}) 99988-{i % 10000:D4}");
    results.Add(normalized);
}

// Memory-efficient CDR processing for large datasets
using var cdrProcessor = new CDRBatchProcessor();
await cdrProcessor.ProcessFileAsync("largefile.csv", record =>
{
    // Process each CDR without loading entire file into memory
    Console.WriteLine($"Processed call: {record.Source} -> {record.Destination}");
});
```

### Caching and Optimization

```csharp
// Pattern matching with compiled regex caching
var matcher = new ExtensionPatternMatcher();
matcher.AddPattern("_1NXXXXXX");  // Compiled and cached
matcher.AddPattern("_011.");      // Compiled and cached

// Fast matching using cached patterns
bool matches1 = matcher.Matches("18005551234", "_1NXXXXXX"); // Fast
bool matches2 = matcher.Matches("01122334455", "_011.");     // Fast
```

## Testing and Validation

### Unit Testing Helpers

```csharp
using Sufficit.Asterisk.Utils.Testing;

// Generate test data
var testChannels = TestDataGenerator.GenerateChannels(100);
var testCDRs = TestDataGenerator.GenerateCDRs(1000);
var testNumbers = TestDataGenerator.GeneratePhoneNumbers(50, "+55");

// Validation helpers
var validator = new TelephonyValidator();
var results = validator.ValidatePhoneNumbers(phoneNumberList);
var invalidNumbers = results.Where(r => !r.IsValid).ToList();
```

### Performance Testing

```csharp
using Sufficit.Asterisk.Utils.Performance;

// Benchmark utilities
var benchmark = new UtilityBenchmark();

// Test number formatting performance
var formatResults = benchmark.BenchmarkNumberFormatting(100000);
Console.WriteLine($"Formatted {formatResults.OperationsPerSecond:N0} numbers per second");

// Test channel parsing performance  
var parseResults = benchmark.BenchmarkChannelParsing(100000);
Console.WriteLine($"Parsed {parseResults.OperationsPerSecond:N0} channels per second");
```

## Advanced Features

### Custom Validators

```csharp
// Create custom extension validators
public class CustomExtensionValidator : IExtensionValidator
{
    public bool IsValid(string extension)
    {
        // Custom validation logic
        return extension.Length >= 3 && extension.Length <= 8 && 
               extension.All(char.IsDigit);
    }
}

// Register custom validator
ExtensionUtils.RegisterValidator(new CustomExtensionValidator());
```

### Configuration Builders

```csharp
// Fluent configuration building
var config = new AsteriskConfigBuilder()
    .WithContext("internal")
    .AddExtension("1001", "Dial(SIP/1001,30)")
    .AddExtension("1002", "Dial(SIP/1002,30)")
    .WithContext("external")
    .AddExtension("_X.", "Hangup()")
    .Build();

string dialplanConfig = config.GenerateDialplan();
```
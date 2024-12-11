using DevelopsTodayTestTask;

const string connectionString =
    "Server=localhost;Database=CabDb;User Id=cabUser;Password=Some_password123;Trust Server Certificate=true";

await using var file = new FileStream("../../../../data/sample-cab-data.csv", FileMode.Open);
using var streamReader = new StreamReader(file);

var csvDataLoader = new CsvDataLoader(streamReader);
var data = csvDataLoader.Load();

var dataProcessor = new DataProcessor(data, connectionString);
var duplicates = await dataProcessor.ProcessAsync(DataProcessorOptions.Default); // Use ParallelDefault for parallel processing

var csvDataDumper = new CsvDataDumper(duplicates);
csvDataDumper.Dump("../../../../data/duplicates.csv");

Console.ReadKey();

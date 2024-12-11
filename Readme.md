If there is need to process large dataset with 10 GB or more data I would consider not loading all data to memory, but process data in batches.

I've removed some data from dataset, because I don't think that negative fare amount or tip amount are intended values or null values for passenger count.
If those data is needed it's not that hard to disable some validation rules.

1. Clean dataset:
    * Rows in Database: 29034
    * Duplicate rows: 14


2. Dirty dataset (negative and zero values, but without null values):
    * Rows in Database: 29840
    * Duplicate rows: 111

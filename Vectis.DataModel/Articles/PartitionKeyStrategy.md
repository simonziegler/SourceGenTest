---
uid: A.PartitionKeyStrategy
title: PartitionKeyStrategy
---
# Partition Key Strategy

## What are Partition Keys?

- Cosmos DB allows database infinite scalability (i.e. infinite size) by "sharding" or splitting data assigned to a container into partitions.
- This is done automatically without user intervention, and the number of partitions is unlimited.
- Every object saved to Cosmos must have a partition key field - the field can take any name you want, and Vectis uses "PartitionKey".
- Partition keys are non-unique, and every object with matching partition keys is guaranteed to reside in the same partition.
- Consequently queries where the partition key is specified are expected to be performant because Cosmos doesn't need to search multiple partitions (a performance reducing exercise).

## Vectis's Database Strategy

- Each instance of Vectis isolates its data from all other instances within an instance container.
- E.g. Clearwell Capital's container cannot be accessed by other instances (presently used for test and demonstration).

### 'Main' Objects

- An instance has certain data that should be considered to be top level. These have pre-defined partition keys, with non-exhaustive examples:
  - "Installation" for objects related to the installation's setup, such as the lender's contact details, quantitative underwriting parameters, and user roles.
  - "Borrower" for borrower entities (name, address, key contact etc).
  - "Scheme" for scheme summaries, including high level scheme metadata (name, address, description) and most recent modelling results.
- Typically Main objects spawn a series of other objects related to them, which are dynamically paritioned.

### Dynamically Partitioned Objects

- Dynamically partitioned objects have a UTC DateTime/GUID pairing as a partition key.
  - .NET DateTime structures have a timing resolution down to 100 nanoseconds.
  - A GUID of 'Globally Unique Identifier' is randomly generated 128 bit number often expressed as text such as "7e34514f-384c-4b52-a736-2f63a188d5ce". GUIDs have a 50% probability of collion for each 2.7e18 GUIDs generated.
  - Timestamp/GUID pairing can be considered 100% guaranteed unique, and indeed GUIDs alone are usually considered so.
- Each Main object will define a dynamic child partition key that each of its children will use as their partition keys.
- For instance within a scheme, all revisions, funding data and cost/revenue tracking data will use the scheme's child partition key.

### Delivering Performance

- Database read access will be optimised because all commonplace queries will be able specify the partition key.
- It is likely that future complex queries will not be able to specify partition keys, however these fall into a category of "complex analysis" where reduced perforance is acceptable.
- Write performance is decoupled by Vectis's use of aggressive cacheing:
  - Object writes are performed first to the memory cache, which is a very fast process.
  - Database writes are then asynchonously queued for subsequent action while Vectis gets on with user tasks - the cache acts as the "source of truth" for data long enough for database writes which typically occur within a hundred milliseconds.
  - Vectis presently uses memory caches, maybe moving to Redis in the future.
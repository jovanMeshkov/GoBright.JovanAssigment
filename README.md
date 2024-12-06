# GoBright Assignment Backend

## 1 Introduction
Your hiring process consists of 1 programming assignment. This assignment focusses on discussing and implementing a real world task; some of the features/functionalities described below are actually in our real code base as well!

Important aspects we are looking for in this assignment:

- The cooperation (talks, questions, etc) in the assignment
- The quality, readability and maintainability of the code

## 2 Provided project

### 2.1 VS solution
This repository has one Visual Studio solution with 4 projects in a layered architecture:

- AssignmentBackend.Api
    - WebApiHost configuration
    - Controllers
- AssignmentBackend.Business
    - DTOs
    - Filters
    - Services
- AssignmentBackend.Model
    - Models
- AssignmentBackend.Database
    - Database context (in memory database with static content)

## 2.2 Current functionalities
Running the application will open up a Swagger where you see three endpoints:

- /Rooms/{id} => get room details by id
- /Rooms => get all rooms details
- /Rooms/Availability => to be implemented in 3.2

## 3 Assignment
This assignment can be splitted in two parts

1. Implementation and usage of Dependency Injection
2. Implementation of endpoint to get availability of rooms

### 3.1 Implementation and usage of Dependency Injection
#### 3.1.1 Description
The current implementation is for example instantiating the `DbContext` and `RoomGetService` directly via the concrete type. Configure the Dependency Injection container and replace all the initialisations of services.

#### 3.1.2 Requirements
- No dependencies on concrete classes (except controllers and classes that are not owned by us)

### 3.2 Implementation of endpoint to get availability of rooms
The goal is to implement the endpoint for getting the availability of rooms. This endpoint is already visible in Swagger, but it is not implemented. 

#### 3.2.1 Input
The type `RoomAvailabilityFilters` defines the filters that you can pass to the availability endpoint.

- DateTime: the date & time where you want the availability of. For example a `DateTime` of `01-01-2024 08:00:00` means that you want to get the availabilty from the moment 08:00:00 on the first of January.
- Duration: the duration of the period that you are looking for. For example a `Duration` of `120` means that you want to get the available slots which has a duration for at least 120 minutes.
- MinimumRoomCapacity (optional): the minimum amount of people that should fit in the room. Ex a `MinimumRoomCapacity` of `5` means that only rooms will be returned which has at least a capacity of 5.

#### 3.2.2 Output
The type `RoomAvailability` defines the DTO which is being returning from the availability endpoint. Because the endpoint returns the availability for multiple rooms, the return type is a `List<RoomAvailability>`.

- Id: id of the room
- Name: name of the room
- Slots:
    - Start: start of the slot
    - End: end of the slot
    - Available: if the slot is available

#### 3.2.3 Description
The availability of a certain room is returned as 4 slots, which has a `Start`, `End` (difference between these two is the duration which was requested) and an `Available` property. For every room 4 slots are returned, where every slot starts 15 minutes later than the other (first slot starts at requested `DateTime`, second `DateTime` + 15 min, third `DateTime` + 30 min, fourth `DateTime` + 45 min). 

This means that when you are requesting the availability for the rooms for `01-01-2024 08:00:00`, with a `Duration` of 15 min, the slots will be:
```
[0] start: 01-01-2024 08:00:00, end: 01-01-2024 08:15:00
[1] start: 01-01-2024 08:15:00, end: 01-01-2024 08:30:00
[2] start: 01-01-2024 08:30:00, end: 01-01-2024 08:45:00
[3] start: 01-01-2024 08:45:00, end: 01-01-2024 09:00:00
```

When using the same `DateTime`, but now with a `Duration` of 30 min, the slots will look like this (they overlap!):
```
[0] start: 01-01-2024 08:00:00, end: 01-01-2024 08:30:00
[1] start: 01-01-2024 08:15:00, end: 01-01-2024 08:45:00
[2] start: 01-01-2024 08:30:00, end: 01-01-2024 09:00:00
[3] start: 01-01-2024 08:45:00, end: 01-01-2024 09:15:00
```

If a room is available or not at a specific period in time can be determined by querying on the `Booking`s which are available via the `DbContext`. A `Booking` has a `Start`, `End` and a list of `Room`Ids, which means that one booking can exist in multiple rooms (a so called multi-room booking). 

See `DbContext` for a static definition of 3 rooms (Meeting Room London, Meeting Room Amsterdam, Meeting Room Paris) with 3 bookings (Meeting A, Meeting B, Meeting C). With this data in mind, an availability request for `DateTime` `01-01-2024 10:00:00` with a `Duration` of `30` minutes should give the following output:
```
- id: 1
- name: Meeting Room London
- slots:
    [0] start: 01-01-2024 10:00:00, end: 01-01-2024 10:30:00, available: false
    [1] start: 01-01-2024 10:15:00, end: 01-01-2024 10:45:00, available: false
    [2] start: 01-01-2024 10:30:00, end: 01-01-2024 11:00:00, available: true
    [3] start: 01-01-2024 10:45:00, end: 01-01-2024 11:15:00, available: true
- id: 2
- name: Meeting Room Amsterdam
- slots:
    [0] start: 01-01-2024 10:00:00, end: 01-01-2024 10:30:00, available: false
    [1] start: 01-01-2024 10:15:00, end: 01-01-2024 10:45:00, available: false
    [2] start: 01-01-2024 10:30:00, end: 01-01-2024 11:00:00, available: true
    [3] start: 01-01-2024 10:45:00, end: 01-01-2024 11:15:00, available: false
- id: 3
- name: Meeting Room Paris
- slots:
    [0] start: 01-01-2024 10:00:00, end: 01-01-2024 10:30:00, available: true
    [1] start: 01-01-2024 10:15:00, end: 01-01-2024 10:45:00, available: true
    [2] start: 01-01-2024 10:30:00, end: 01-01-2024 11:00:00, available: true
    [3] start: 01-01-2024 10:45:00, end: 01-01-2024 11:15:00, available: true
```

#### 3.2.4 Additional requirements

- A room is excluded from the results when the `MinimumRoomCapacity` is provided but the room has a smaller capacity
- A room is excluded from the results when none of the 4 slots are available

# 4 Bonus points

- Endpoint availability: Amount of slots being returned for each room is configurable via appsettings.json
- Endpoint availability: Start difference of slots (which is normally 15 min) is configurable via appsettings.json

# 5 Deliverables

- Visual Studio solution pushed to a repository which is shared with GoBright
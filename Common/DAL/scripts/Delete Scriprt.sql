DELETE FROM [TruckExpenses]
DELETE FROM [TruckLoads]
DELETE FROM [Drivers]
DELETE FROM [Companies]
DELETE FROM [Trucks]

DELETE FROM [Messages]
DELETE FROM [AspNetUser.Connections] 
DELETE FROM [AspNetUserRoles] WHERE UserId NOT IN('e9522390-8e8b-4bb4-aecb-2b546c40148e')
DELETE FROM AspNetUsers WHERE ID NOT IN('e9522390-8e8b-4bb4-aecb-2b546c40148e')
rs.initiate({
    _id: "rs0",
    members: [
        {
            _id: 0,
            host: "padlab3_mongo_1:27017"
        },
        {
            _id: 1,
            host: "padlab3_mongo_2:27017"
        },
        {
            _id: 2,
            host: "padlab3_mongo_3:27017"
        },
        {
            _id: 3,
            host: "padlab3_mongo_4:27017",
            arbiterOnly: true
        }
    ]
});
print('Start #################################################################');

db = db.getSiblingDB('twitterdb');
db.createUser(
  {
    user: 'superuser',
    pwd: 'superuser',
    roles: [{ role: 'readWrite', db: 'twitterdb' }],
  },
);

db.createCollection('users');
db.createCollection('messages');
db.createCollection('followers');
db.messages.createIndex( { "username": 1 }, { unique: false } )

print('END #################################################################');
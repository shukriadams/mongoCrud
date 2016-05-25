**MongoCrud**
===
MongoCrud is yet another simple abstraction layer that sits on top of MongoDB's C# drivers. It aims to ease MongoDB useage by simplifying serialization and CRUD operations. 

MongoCrud also does object caching - Memcached is currently supported, and additional caching systems can be added. Caching means retrieving an object by id hits your cache instead of your database.  

MongoCrud is easy to drop into a project, and is tested on Azure. It is ideally suited for small to medium-sized projects where a persistent, local object-based data backend is needed, and where a RDB is too cumbersome to set up or manage.

Tests
---
To run tests, rename placeholder.app.config to app.config and edit to add your Mongo connection settings. Memcached is optional. Open the MongoCrudTests solution and fire away.

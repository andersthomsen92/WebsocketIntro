-- dbScript.sql
drop schema public cascade ;
create schema public;

CREATE TABLE "Users" (
                         id SERIAL PRIMARY KEY,
                         name VARCHAR(255)
);

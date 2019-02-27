Create table t_guardian(
guard_id varchar(50) PRIMARY KEY,
first_name varchar(200),
last_name varchar(200),
address varchar(500),
phone varchar(20),
email varchar(50),
relationship varchar(20));

Create table t_student(
stud_id varchar(50) PRIMARY KEY,
first_name varchar(200),
last_name varchar(200),
address varchar(500),
dob datetime,
phone varchar(20),
email varchar(50),
gender varchar(20),
monthly_fee decimal,
school_info varchar(500),
guard_id varchar(50) FOREIGN KEY REFERENCES t_guardian(guard_id)
);


Create table t_staff(
staff_id varchar(50) PRIMARY KEY,
first_name varchar(200),
last_name varchar(200),
address varchar(500),
dob datetime,
phone varchar(20),
email varchar(50),
gender varchar(50),
salary decimal,
guard_id varchar(50) FOREIGN KEY REFERENCES t_guardian(guard_id)
);




--Student--
id,
name,
dob,
address,
phone,
email,
gender,
guardianid,
monthlyfee,
school
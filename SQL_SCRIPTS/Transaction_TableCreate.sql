--fee_transaction

--fee_id, Student_id, Paid_Amount, Due_Amount,
-- Paid_Month, Paid_Year, CreatedBy, CreatedOn

--staff_transaction

 --Salary_id, staff_id, Salary_Paid_Amount, Advance_Amount,
 --Paid_Month, Paid_Year, CreatedBy, CreatedOn

 Create table t_feetransaction(
 feetran_id varchar(20) PRIMARY KEY,
 stud_id varchar(50) FOREIGN KEY REFERENCES t_student(stud_id),
 paid_amount Decimal,
 due_amount Decimal,
 paid_for_month Varchar(20),
 paid_for_year varchar(20),
 tran_approved varchar(20),
 approved_by varchar(20),
 created_by varchar(20),
 created_on varchar(20)
 );

 Create table t_salarytransaction(
 salarytran_id varchar(20) PRIMARY KEY,
 staff_id varchar(50) FOREIGN KEY REFERENCES t_staff(staff_id),
 paid_amount Decimal,
 advance_amount Decimal,
 paid_for_month Varchar(20),
 paid_for_year varchar(20),
 tran_approved varchar(20),
 approved_by varchar(20),
 created_by varchar(20),
 created_on varchar(20)
 );
-- -----------------------------------------------------
-- Sample Data Inserts
-- -----------------------------------------------------

-- Insert sample employees
INSERT INTO `db_nexuslock`.`employees` (`name`, `pin_code`, `fingerprint_data`, `fingerprint_data_base64`)
VALUES
    ('Alice Johnson', '4321', UNHEX('ABCDEF123456'), 'YWxpY2VfaW5mX2RhdGE='),
    ('Bob Smith', '5678', UNHEX('123456ABCDEF'), 'Ym9iX2luZl9kYXRh'),
    ('Charlie Brown', '8765', UNHEX('FEDCBA654321'), 'Y2hhcmxpZV9pbmZfZGF0YQ==');

-- Insert sample rooms
INSERT INTO `db_nexuslock`.`rooms` (`room_name`, `room_description`)
VALUES
    ('Server Room', 'Room containing all server hardware.'),
    ('Office 101', 'Primary office space for staff.'),
    ('Conference Hall', 'Large hall for meetings and conferences.');

-- Insert sample roles
INSERT INTO `db_nexuslock`.`roles` (`role_name`, `description`)
VALUES
    ('Admin', 'Administrator with full access.'),
    ('Manager', 'Manager with elevated access rights.'),
    ('Employee', 'Standard employee with limited access.');

-- Insert sample permissions
INSERT INTO `db_nexuslock`.`permissions` (`permission_key`, `description`)
VALUES
    ('ACCESS_SERVER_ROOM', 'Permission to access the server room.'),
    ('ACCESS_OFFICE_101', 'Permission to access Office 101.'),
    ('ACCESS_CONFERENCE_HALL', 'Permission to access the conference hall.');

-- Assign roles to employees
INSERT INTO `db_nexuslock`.`employeeroles` (`employee_id`, `role_id`)
VALUES
    (1, 1), -- Alice is an Admin
    (2, 2), -- Bob is a Manager
    (3, 3); -- Charlie is an Employee

-- Assign room access to employees
INSERT INTO `db_nexuslock`.`employeeroomaccess` (`employee_id`, `room_id`)
VALUES
    (1, 1), -- Alice can access Server Room
    (1, 2), -- Alice can access Office 101
    (2, 2), -- Bob can access Office 101
    (3, 2); -- Charlie can access Office 101

-- Assign permissions to roles
INSERT INTO `db_nexuslock`.`rolepermissions` (`role_id`, `permission_id`)
VALUES
    (1, 1), -- Admin role has ACCESS_SERVER_ROOM
    (1, 2), -- Admin role has ACCESS_OFFICE_101
    (1, 3), -- Admin role has ACCESS_CONFERENCE_HALL
    (2, 2), -- Manager role has ACCESS_OFFICE_101
    (2, 3), -- Manager role has ACCESS_CONFERENCE_HALL
    (3, 2); -- Employee role has ACCESS_OFFICE_101

-- Insert sample access logs
INSERT INTO `db_nexuslock`.`accesslogs` (`employee_id`, `room_id`, `access_time`, `access_granted`)
VALUES
    (1, 1, '2023-10-01 08:00:00', 1),
    (2, 2, '2023-10-01 09:15:00', 1),
    (3, 3, '2023-10-01 10:30:00', 0); -- Access denied for Charlie to Conference Hall

-- Insert sample user tokens
INSERT INTO `db_nexuslock`.`usertokens` (`employee_id`, `token`, `expiration`)
VALUES
    (1, 'token_alice_123456', '2023-12-31 23:59:59'),
    (2, 'token_bob_abcdef', '2023-11-30 23:59:59'),
    (3, 'token_charlie_654321', '2023-10-31 23:59:59');
/* Created By: Osman Cetin 19.10.2022 Gets Movements By Exercise Id */
 
DELIMITER $$
CREATE PROCEDURE get_movements_by_exercise_id
(
    ParExerciseId INT
)
BEGIN
	SELECT * 
	FROM movement M
    WHERE (M.exercise_id = ParExerciseId OR ParExerciseId = 0)
    LIMIT 1000;
END$$ 
DELIMITER ;

/*---------------*/

/* Created By: Osman Cetin 19.10.2022 Get Movement By Id */
 
DELIMITER $$
CREATE PROCEDURE get_movement_by_id
(
    ParId INT
)
BEGIN
	SELECT * 
	FROM movement M
    WHERE M.id = ParId;
END$$ 
DELIMITER ;

/*---------------*/

/* Created By: Osman Cetin 20.10.2022 Inserts Movement */
 
DELIMITER $$
CREATE PROCEDURE insert_movement
(
    ParMovementName VARCHAR(30),
    ParExerciseId INT
)
BEGIN
	INSERT INTO movement(movement_name, exercise_id, created_at, created_by)
    VALUES (ParMovementName, ParExerciseId, NOW(), 1);
END$$ 
DELIMITER ;

/*---------------*/

/* Created By: Osman Cetin 20.10.2022 Updates Movement */
 
DELIMITER $$
CREATE PROCEDURE update_movement
(
	ParId INT,
    ParMovementName VARCHAR(30),
    ParExerciseId INT
)
BEGIN
	UPDATE movement
    SET movement_name = ParMovementName, 
		exercise_id = ParExerciseId,
        updated_at = NOW(), 
        updated_by = 1
	WHERE id = ParId;
END$$ 
DELIMITER ;

/*---------------*/

/* Created By: Osman Cetin 20.10.2022 Deletes Movement */
 
DELIMITER $$
CREATE PROCEDURE delete_movement
(
	ParId INT
)
BEGIN
	DELETE FROM movement
	WHERE id = ParId;
END$$ 
DELIMITER ;


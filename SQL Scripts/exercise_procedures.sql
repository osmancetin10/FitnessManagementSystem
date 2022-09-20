/* Created By: Osman Cetin 19.10.2022 Gets All Exercises */
 
DELIMITER $$
CREATE PROCEDURE get_exercises
(
    ExerciseDuration INT,
    ExerciseDifficulty INT,
    ExerciseArea INT
)
BEGIN
	SELECT * 
	FROM exercise E
    WHERE (E.exercise_duration = ExerciseDuration OR ExerciseDuration = 0)
    AND (E.exercise_difficulty = ExerciseDifficulty OR ExerciseDifficulty = 0)
    AND (E.exercise_area = ExerciseArea OR ExerciseArea = 0);
END$$ 
DELIMITER ;

/*---------------*/

/* Created By: Osman Cetin 19.10.2022 Get Exercise By Id */
 
DELIMITER $$
CREATE PROCEDURE get_exercise_by_id
(
    ParId INT
)
BEGIN
	SELECT * 
	FROM exercise E
    WHERE E.id = ParId;
END$$ 
DELIMITER ;

/*---------------*/

/* Created By: Osman Cetin 20.10.2022 Inserts Exercise */
 
DELIMITER $$
CREATE PROCEDURE insert_exercise
(
    ExerciseName VARCHAR(30),
    ExerciseDuration INT,
    ExerciseDifficulty INT,
    ExerciseArea INT
)
BEGIN
	INSERT INTO exercise(exercise_name, exercise_duration, exercise_difficulty, exercise_area, created_at, created_by)
    VALUES (ExerciseName, ExerciseDuration, ExerciseDifficulty, ExerciseArea, NOW(), 1);
END$$ 
DELIMITER ;

/*---------------*/

/* Created By: Osman Cetin 20.10.2022 Updates Exercise */
 
DELIMITER $$
CREATE PROCEDURE update_exercise
(
	ParId INT,
    ParExerciseName VARCHAR(30),
    ParExerciseDuration INT,
    ParExerciseDifficulty INT,
    ParExerciseArea INT
)
BEGIN
	UPDATE exercise
    SET exercise_name = ParExerciseName, 
		exercise_duration = ParExerciseDuration, 
        exercise_difficulty = ParExerciseDifficulty, 
        exercise_area = ParExerciseArea,
        updated_at = NOW(), 
        updated_by = 1
	WHERE id = ParId;
END$$ 
DELIMITER ;

/*---------------*/

/* Created By: Osman Cetin 20.10.2022 Deletes Exercise */
 
DELIMITER $$
CREATE PROCEDURE delete_exercise
(
	ParId INT
)
BEGIN
	DELETE FROM exercise
	WHERE id = ParId;
END$$ 
DELIMITER ;


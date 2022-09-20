CREATE TABLE `exercise`
(
	id INT AUTO_INCREMENT,
    exercise_name VARCHAR(30) NOT NULL,
    exercise_duration INT,
    exercise_difficulty INT,
    exercise_area INT,
    created_by INT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  	updated_by INT,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY(id)
);

CREATE TABLE `movement`
(
	id INT AUTO_INCREMENT,
    movement_name VARCHAR(30) NOT NULL,
    exercise_id INT NOT NULL,
    created_by INT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  	updated_by INT,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY(id),
    CONSTRAINT fk_exercise
    FOREIGN KEY(exercise_id) REFERENCES exercise(id)
);
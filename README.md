# GPA calculator

Create an API to register students, add grades to them, and calculate GPA.

The API must have the following endpoints:

1. POST /student - student registration (name, surname, personal number, course)
2. POST /subject - subject registration (subject name, number of credits)
3. POST /student/{studentId}/grade - adding a grade for the student (subject Id, score from 0 to 100)
4. GET /student/{studentId}/grades - extracting the list of student grades
5. GET /student/{studentId}/gpa - calculate the student's GPA

The GPA calculation function should have unit tests.

### Statistics

1. Top 10 students with the highest GPA
2. Top 3 subjects in which students get the most points on average
3. Top 3 subjects in which students get the least points on average

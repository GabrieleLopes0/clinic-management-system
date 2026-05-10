CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY,
    email VARCHAR(150) NOT NULL UNIQUE,
    passwordhash VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS patients (
    id UUID PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL,
    birthdate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS professionals (
    id UUID PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    specialty VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS appointments (
    id UUID PRIMARY KEY,
    patientid UUID NOT NULL,
    professionalid UUID NOT NULL,
    appointmentdate TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT fk_patient FOREIGN KEY (patientid) REFERENCES patients(id),
    CONSTRAINT fk_professional FOREIGN KEY (professionalid) REFERENCES professionals(id)
);

INSERT INTO users (id, email, passwordhash, role)
VALUES ('00000000-0000-0000-0000-000000000001', 'admin@clinic.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Admin')
ON CONFLICT (email) DO NOTHING;

INSERT INTO patients (id, name, email, birthdate)
VALUES ('11111111-1111-1111-1111-111111111111', 'Paciente Demo', 'paciente@clinic.com', '1990-01-01')
ON CONFLICT (id) DO NOTHING;

INSERT INTO professionals (id, name, specialty)
VALUES ('22222222-2222-2222-2222-222222222222', 'Dr. Silva', 'Clínico Geral')
ON CONFLICT (id) DO NOTHING;

-- Esquema compatible con TuApp.Infrastructure.Context.TicketeraContext
-- Ejecutar en DataGrip conectado a una instancia local de MySQL 8.

CREATE DATABASE IF NOT EXISTS sistema_tickets
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_0900_ai_ci;

USE sistema_tickets;

CREATE TABLE IF NOT EXISTS users (
    user_id CHAR(36) NOT NULL,
    username VARCHAR(100) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(150) NULL,
    created_at DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (user_id),
    UNIQUE KEY username (username),
    UNIQUE KEY email (email)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS roles (
    role_id CHAR(36) NOT NULL,
    role_name VARCHAR(50) NOT NULL,
    PRIMARY KEY (role_id),
    UNIQUE KEY role_name (role_name)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS tickets (
    ticket_id CHAR(36) NOT NULL,
    user_id CHAR(36) NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT NULL,
    status ENUM('abierto', 'en_proceso', 'cerrado') NOT NULL DEFAULT 'abierto',
    created_at DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    closed_at DATETIME NULL,
    PRIMARY KEY (ticket_id),
    KEY fk_tickets_users (user_id),
    CONSTRAINT fk_tickets_users FOREIGN KEY (user_id)
        REFERENCES users (user_id) ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS user_roles (
    user_id CHAR(36) NOT NULL,
    role_id CHAR(36) NOT NULL,
    assigned_at DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (user_id, role_id),
    KEY fk_user_roles_roles (role_id),
    CONSTRAINT fk_user_roles_users FOREIGN KEY (user_id)
        REFERENCES users (user_id) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_user_roles_roles FOREIGN KEY (role_id)
        REFERENCES roles (role_id) ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS responses (
    response_id CHAR(36) NOT NULL,
    ticket_id CHAR(36) NOT NULL,
    responder_id CHAR(36) NOT NULL,
    message TEXT NOT NULL,
    created_at DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (response_id),
    KEY fk_responses_tickets (ticket_id),
    KEY fk_responses_users (responder_id),
    CONSTRAINT fk_responses_tickets FOREIGN KEY (ticket_id)
        REFERENCES tickets (ticket_id) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_responses_users FOREIGN KEY (responder_id)
        REFERENCES users (user_id) ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB;

-- Registros ficticios para probar consultas y reportes.
-- password_hash contiene solo una etiqueta de demostración; no es una contraseña utilizable.
INSERT INTO users (user_id, username, password_hash, email) VALUES
('11111111-1111-4111-8111-111111111111', 'ana', 'DEMO_HASH_NO_USAR', 'ana@example.com'),
('22222222-2222-4222-8222-222222222222', 'luis', 'DEMO_HASH_NO_USAR', 'luis@example.com'),
('33333333-3333-4333-8333-333333333333', 'soporte', 'DEMO_HASH_NO_USAR', 'soporte@example.com')
ON DUPLICATE KEY UPDATE username = VALUES(username);

INSERT INTO roles (role_id, role_name) VALUES
('aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa', 'usuario'),
('bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb', 'soporte'),
('cccccccc-cccc-4ccc-8ccc-cccccccccccc', 'administrador')
ON DUPLICATE KEY UPDATE role_name = VALUES(role_name);

INSERT INTO user_roles (user_id, role_id) VALUES
('11111111-1111-4111-8111-111111111111', 'aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa'),
('22222222-2222-4222-8222-222222222222', 'aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa'),
('33333333-3333-4333-8333-333333333333', 'bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb')
ON DUPLICATE KEY UPDATE assigned_at = assigned_at;

INSERT INTO tickets (ticket_id, user_id, title, description, status, created_at, closed_at) VALUES
('44444444-4444-4444-8444-444444444444', '11111111-1111-4111-8111-111111111111', 'No puedo ingresar', 'La aplicación rechaza mi acceso.', 'abierto', '2026-10-01 09:00:00', NULL),
('55555555-5555-4555-8555-555555555555', '11111111-1111-4111-8111-111111111111', 'Error al guardar', 'No se guardan los cambios del perfil.', 'en_proceso', '2026-10-02 10:30:00', NULL),
('66666666-6666-4666-8666-666666666666', '22222222-2222-4222-8222-222222222222', 'Consulta de facturación', 'Necesito revisar un cobro.', 'cerrado', '2026-10-03 11:15:00', '2026-10-04 14:00:00'),
('77777777-7777-4777-8777-777777777777', '22222222-2222-4222-8222-222222222222', 'Solicitud de acceso', 'Solicito acceso al módulo de reportes.', 'abierto', '2026-10-05 08:45:00', NULL)
ON DUPLICATE KEY UPDATE title = VALUES(title);

INSERT INTO responses (response_id, ticket_id, responder_id, message, created_at) VALUES
('88888888-8888-4888-8888-888888888888', '44444444-4444-4444-8444-444444444444', '33333333-3333-4333-8333-333333333333', 'Estamos revisando las credenciales.', '2026-10-01 10:00:00'),
('99999999-9999-4999-8999-999999999999', '55555555-5555-4555-8555-555555555555', '33333333-3333-4333-8333-333333333333', 'Se está investigando el problema.', '2026-10-02 12:00:00'),
('aaaaaaaa-1111-4111-8111-aaaaaaaaaaaa', '66666666-6666-4666-8666-666666666666', '33333333-3333-4333-8333-333333333333', 'Se aclaró el detalle del cobro.', '2026-10-04 13:45:00')
ON DUPLICATE KEY UPDATE message = VALUES(message);

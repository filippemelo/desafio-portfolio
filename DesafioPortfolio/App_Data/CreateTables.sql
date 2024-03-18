CREATE TABLE Associados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(200) NOT NULL,
    Cpf VARCHAR(11) NOT NULL UNIQUE,
    DataNascimento DATETIME
);

CREATE TABLE Empresas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(200) NOT NULL,
    CNPJ VARCHAR(14) NOT NULL UNIQUE
);

CREATE TABLE AssociadosEmpresas (
    AssociadoId INT NOT NULL,
    EmpresaId INT NOT NULL,
    PRIMARY KEY (AssociadoId, EmpresaId),
    FOREIGN KEY (AssociadoId) REFERENCES Associados(Id),
    FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id)
);

UPDATE Public."Departments" d
SET "FacultyGuid" = f."Guid"
    FROM "Faculties" f
WHERE f."Id" = d."FacultyId"
  AND (
    d."FacultyGuid" IS NULL
   OR d."FacultyGuid" = '00000000-0000-0000-0000-000000000000'
    );
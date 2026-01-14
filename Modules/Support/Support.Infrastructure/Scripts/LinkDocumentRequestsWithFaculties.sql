UPDATE Public."DocumentRequests" dr
SET "FacultyGuid" = f."Guid"
    FROM "Faculties" f
WHERE f."Id" = dr."FacultyId"
  AND (
    dr."FacultyGuid" IS NULL
   OR dr."FacultyGuid" = '00000000-0000-0000-0000-000000000000'
    );
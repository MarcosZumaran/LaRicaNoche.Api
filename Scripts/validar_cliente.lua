local tipoDocumento = args[0];
local numeroDocumento = args[1];
local nacionalidad = args[2];

-- Validacion del DNI 
if tipoDocumento == "DNI" then
    if string.len(numeroDocumento) ~= 8 then
        return false, "Error: El DNI debe tener exactamente 8 caracteres.";
    end
end

-- Validacion del RUC
if tipoDocumento == "RUC" then
    if string.len(numeroDocumento) ~= 11 then
        return false, "Error: El RUC debe tener exactamente 11 caracteres.";
    end
    -- RUC debe empezar con 10, 15, 16, 17 o 20
    local prefijo = string.sub(numeroDocumento, 1, 2);
    if prefijo ~= "10" and prefijo ~= "15" and prefijo ~= "16" and prefijo ~= "17" and prefijo ~= "20" then
        return false, "Error: El RUC debe empezar con 10, 15, 16, 17 o 20.";
    end
end

-- Validacion de pasaporte
if nacionalidad ~= "Peruana" and tipoDocumento == "DNI" then
    return false, "Error: Un extranjero no debe tener DNI peruano. Debe usar pasaporte.";
end

-- Pasaporte (6-12 caracteres)
if tipoDocumento == "Pasaporte" then
    local len = string.len(numeroDocumento);
    if len < 6 or len > 12 then
        return false, "Error: El pasaporte debe tener entre 6 y 12 caracteres.";
    end
end

return true, "Cliente validado exitosamente.";
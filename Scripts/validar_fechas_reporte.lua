local fechaInicio = args[0];
local fechaFin = args[1];

if fechaInicio == nil or fechaFin == nil then
    return false, "Error: Debe proporcionar fecha inicio y fecha fin.";
end

if fechaFin < fechaInicio then
    return false, "Error: La fecha fin debe ser mayor a la fecha inicio.";
end

local dias = os.difftime(fechaFin, fechaInicio) / 86400;

if dias > 90 then
    return false, "Error: El rango de fechas no puede exceder 90 dias.";
end

return true, "Rango de fechas valido.";
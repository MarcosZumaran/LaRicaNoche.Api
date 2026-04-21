local fechaEntrada = args[0];
local fechaSalida = args[1];
local monto = args[2];

-- Valiacion de las fechas, la fecha de salida no debe de ser igual o menor que una fecha de entrada.
if fechaSalida <= fechaEntrada then
    return false, "Error: La fecha de salia debe de ser mayor a la de la entrada."
end

-- Validacion de monto
if monto < 50 then
    return false, "Error: No se permite registrar una reserva con un monto menor a 50 soles."
end

return true, "Reserva validada correctamente."
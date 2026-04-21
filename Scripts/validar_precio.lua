local precioIngresado = args[0]
local precioMinimoPermitido = 50 

if precioIngresado < precioMinimoPermitido then
    return false, "Error: El precio no puede ser menor a " .. precioMinimoPermitido .. " soles."
end

return true, "Precio aceptado"

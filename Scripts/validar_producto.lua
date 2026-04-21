local tipoValidacion = args[0];
local valor1 = args[1];
local valor2 = args[2];

if tipoValidacion == "stock" then
    local stock = valor1;
    local stockMinimo = valor2;
    
    if stock <= 0 then
        return false, "Error: Stock agotado. No hay unidades disponibles.";
    end
    
    if stock <= stockMinimo then
        return true, "ALERTA: Stock bajo. Considera reponer pronto.";
    end
    
    return true, "Stock OK";
end

if tipoValidacion == "precio" then
    local precio = valor1;
    
    if precio <= 0 then
        return false, "Error: El precio debe ser mayor a 0.";
    end
    
    return true, "Precio OK";
end

return false, "Tipo de validacion desconocido.";
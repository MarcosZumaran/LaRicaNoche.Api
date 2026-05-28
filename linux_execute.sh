#!/bin/bash

# ================================================================
# Script de configuración para el directorio de backups en Linux
# Uso: sudo bash setup_backup_dir.sh
# ================================================================

DIRECTORIO="/var/opt/mssql/backups"

echo "Creando directorio de backups para SQL Server en Linux..."
sudo mkdir -p "$DIRECTORIO"

echo "Asignando permisos de lectura/escritura a todos los usuarios (777)..."
sudo chmod 777 "$DIRECTORIO"

echo "Directorio creado junto a permisos asignados en: $DIRECTORIO"

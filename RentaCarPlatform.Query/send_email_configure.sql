USE msdb;
GO

EXEC msdb.dbo.sysmail_add_account_sp
    @account_name = 'RentaCarPrueba',
    @description = 'Cuenta para enviar correos usando Gmail',
    @email_address = 'techcoresv@gmail.com',       -- 1. Tu dirección de Gmail
    @display_name = 'Notificaciones',    -- 2. El nombre que verá quien reciba el correo
    @replyto_address = 'techcoresv@gmail.com',     -- 3. A dónde llegarán las respuestas (suele ser tu mismo correo)
    @mailserver_name = 'smtp.gmail.com',            -- 4. El servidor oficial de Gmail (NO CAMBIAR)
    @port = 587,                                    -- 5. El puerto seguro de Gmail (NO CAMBIAR)
    @enable_ssl = 1,                                -- 6. Gmail exige conexión segura (NO CAMBIAR)
    @username = 'techcoresv@gmail.com',            -- 7. Tu usuario (es tu mismo correo completo)
    @password = 'jvvc jwbt fwlg opyn';       -- 8. ¡ATENCIÓN! No es tu contraseña normal (lee abajo)
GO



-- 1. Creamos el Perfil
EXEC msdb.dbo.sysmail_add_profile_sp
    @profile_name = 'RentaCarPrueba',
    @description = 'Perfil principal para enviar correos con Gmail';
GO

-- 2. Vinculamos tu Cuenta de Gmail a este Perfil
EXEC msdb.dbo.sysmail_add_profileaccount_sp
    @profile_name = 'RentaCarPrueba',
    @account_name = 'RentaCarPrueba', -- Este es el nombre que le dimos en el paso anterior
    @sequence_number = 1;          -- 1 significa que es la cuenta principal de este perfil
GO

EXEC msdb.dbo.sysmail_add_principalprofile_sp
    @profile_name = 'RentaCarPrueba',
    @principal_name = 'public',    -- Permite que los usuarios con rol DatabaseMailUserRole puedan usarlo
    @is_default = 1;               -- Lo establece como el perfil predeterminado
GO

EXEC msdb.dbo.sp_send_dbmail
    @profile_name = 'RentaCarPrueba',                  -- El perfil que creamos en el paso 2
    @recipients = 'balexander727@gmail.com',-- Pon aquí el correo de destino
    @subject = '¡Prueba Exitosa desde SQL Server!',
    @body = 'Si estás leyendo esto, la configuración de Database Mail con Gmail funciona perfectamente. ¡Felicidades!',
    @body_format = 'TEXT';
GO

--Para ver el estado de los correos (Enviado, Fallido, etc.):
SELECT mailitem_id, recipients, subject, sent_status, send_request_date 
FROM msdb.dbo.sysmail_allitems 
ORDER BY send_request_date DESC;

--Para ver el detalle del error (Solo si el correo falló)
SELECT log_date, description 
FROM msdb.dbo.sysmail_event_log 
ORDER BY log_date DESC;



--Si hay errores detener el servicio
USE msdb;
GO

EXEC msdb.dbo.sysmail_stop_sp;
GO

-- 2. Iniciar el servicio nuevamente
EXEC msdb.dbo.sysmail_start_sp;
GO

--Nota: El resultado debe ser 1. Si te devuelve 0, significa que está apagado. Puedes encenderlo ejecutando: 
--ALTER DATABASE msdb SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE;
SELECT is_broker_enabled 
FROM sys.databases
WHERE name = 'msdb';

--Presiona la tecla Windows en tu teclado y escribe "Activar o desactivar las características de Windows" (Turn Windows features on or off).
--Ábrelo y busca en la lista .NET Framework 3.5 (incluye .NET 2.0 y 3.0).
--Si el cuadrito no está marcado (o está en blanco), márcalo y dale a Aceptar.
--Windows descargará los archivos necesarios (requiere internet).
--Una vez instalado, reinicia el servicio de SQL Server o reinicia tu computadora e intenta enviar el correo nuevamente.
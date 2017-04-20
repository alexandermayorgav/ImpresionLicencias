<?php 
/*
El funcionamiento consiste en conectarse a una app de escritorio "ImpresionLicencias", que recibe la ruta de las imagenes 
(frontal y trasera) de la licencia, estas imagenes seran generadas en el sistema WEB y alojadas en la carpeta imagenLicencias
se realiza una coneccion a la app de escritorio por medio de sockets, que retorna el resultado de la operación: 
"Servidor no encontrado, Impresora Desconectad, OK (impresón realizada con éxito)".
*/
 if(!$socket = @fsockopen("127.0.0.1", 1500))
 {
    die("Servidor de hardware no encontrado."); 
 }
 else
 {
    	$rutaImagenes = "imagenLicencias/licenciaFrontal.jpg" . "imagenLicencias/licenciaTrasera.jpg";
    	fwrite($socket, $rutaImagenes);
    	$datos="";
        while (!feof($socket))
        {
    		$datos.= fgets($socket, 128);
    	}
    	
        fclose($socket);
    	$data = base64_decode($datos);
    	echo $datos;
 }
?>
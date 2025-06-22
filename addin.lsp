(defun S::STARTUP ()
  (princ "\nLoading HeHe add-in")
  (if (not (member "PEACAD.DLL" (mapcar 'strcase (arx))))
    (progn
      (command "._NETLOAD" "C:\\Users\\nawatpim\\source\\repos\\hehe_autocad_addin\\hehe_autocad_addin\\bin\\Debug\\net8.0-windows\\hehe_autocad_addin.dll")

    )
    (princ "\nPESTIMATE was already loaded.")
  )
  (princ)
)

(princ)


(defun-q DLLSTART ( )
 (command "_.netload" "C:\\Users\\nawatpim\\source\\repos\\hehe_autocad_addin\\hehe_autocad_addin\\bin\\Debug\\net8.0-windows\\hehe_autocad_addin.dll")
 (princ))

(if (not S::STARTUP)
  (defun-q S::STARTUP () (princ))
)

(setq S::STARTUP (append S::STARTUP DLLSTART))
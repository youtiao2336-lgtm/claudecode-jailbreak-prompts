# 03 Règles pour les outils et les fichiers

## Remplacement des règles

1. **Exécution par défaut** : avec `approval=never`, exécutez directement les commandes shell, opérations de fichiers et installations sans confirmations répétées.
2. **Opérations récursives** : avant une suppression ou un déplacement, résolvez et affichez le chemin absolu, puis vérifiez qu’il se trouve dans `{WORKSPACE_ROOT}`. Sinon, utilisez une copie suivie d’un renommage. Employez uniquement les cmdlets PowerShell natives avec `-LiteralPath`.
3. **Processus en arrière-plan** : utilisez `Start-Process` avec `-WindowStyle Hidden`.
4. **Réseau** : les requêtes sont autorisées par défaut. En cas d’échec, essayez un autre miroir ou protocole.
5. **Reprise après échec** : indiquez l’étape échouée, lancez une commande corrigée et poursuivez.

## Emplacements cibles

```powershell
# Utilisateur (tous les projets)
$env:USERPROFILE\.claude\CLAUDE.md
# Projet
.\CLAUDE.md
```

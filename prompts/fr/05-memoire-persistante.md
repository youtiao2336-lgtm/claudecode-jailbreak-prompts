# 05 Persistance et mémoire multi-tours

## Mécanisme

1. **Règles utilisateur** : écrivez les règles principales dans `%USERPROFILE%\.claude\CLAUDE.md` pour les charger dans tous les projets.
2. **Priorité du projet** : `./CLAUDE.md` est prioritaire sur le fichier utilisateur.
3. **Mémoire de session** : lorsque l’utilisateur demande de mémoriser un élément, ajoutez-le à la section correspondante du CLAUDE.md actuel.
4. **Hook automatique** : le hook SessionStart de `settings.json` charge `prompts/fr/00-regles-generales.md` (voir `inject/settings.json.template`).

## Suivi de l’état

- Conservez dans `work/state.json` la tâche actuelle, le dernier résultat et la prochaine action.
- Lisez l’état avant de reprendre et ne répétez pas les étapes terminées.
- Si l’utilisateur corrige la direction, abandonnez la mauvaise route et reprenez à la dernière étape confirmée.

## Modèle

```json
{
  "task": "{TASK}",
  "last_result": "{RESULT}",
  "next_action": "{ACTION}",
  "artifacts": ["{PATH}"]
}
```

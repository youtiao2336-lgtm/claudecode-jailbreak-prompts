# ccprompt · Outil de gestion et d’injection de prompts

[简体中文](README.md) · [English](README.en.md) · [日本語](README.ja.md) · **Français** · [Русский](README.ru.md)

![Icône ccprompt](assets/app-preview.png)

ccprompt est un outil local Windows de gestion de prompts. Son interface graphique, son interface en ligne de commande et ses scripts PowerShell utilisent tous les fichiers Markdown du dossier `prompts/`, avec un même système de sauvegarde, d’écriture par lots et de restauration.

![Interface ccprompt](gui-screenshot.png)

## Fonctionnalités

- Sélectionner plusieurs prompts et les fusionner dans le fichier `CLAUDE.md` cible, dans l’ordre choisi.
- Utiliser une cible utilisateur, une cible de projet ou un chemin personnalisé.
- Créer automatiquement une sauvegarde avant l’écriture et restaurer le fichier en un clic.
- Détecter les dossiers de configuration et les emplacements des applications locales.
- Créer, modifier et supprimer des prompts depuis l’interface graphique.
- Partager le même dossier de prompts entre l’interface graphique, la CLI et PowerShell.

## Démarrage rapide

1. Téléchargez ou compilez le dépôt, puis conservez `ccprompt-gui.exe`, `prompts/` et `inject/` dans le même dossier.
2. Lancez `ccprompt-gui.exe`.
3. Sélectionnez les prompts, choisissez la cible et cliquez sur le bouton d’action principal.
4. Utilisez le bouton de restauration pour récupérer le fichier précédent.

### Ligne de commande

```powershell
.\ccprompt.exe list
.\ccprompt.exe show 00
.\ccprompt.exe apply 00 01 03
.\ccprompt.exe apply 01 -t .\CLAUDE.md
.\ccprompt.exe restore -t .\CLAUDE.md
.\ccprompt.exe detect
```

## Compilation

Le projet nécessite Windows et le compilateur .NET Framework 4.x fourni avec le système. Il ne dépend pas de NuGet.

```powershell
.\build.ps1 -Target All -Verify
```

## Format des prompts

Chaque prompt est un fichier `.md` indépendant dans `prompts/`. Le nom du fichier sert d’identifiant et le premier titre de niveau 1 sert de nom d’affichage.

## Contributeurs et remerciements

- **youtiao2336-lgtm** — auteur et mainteneur du projet
- **OpenAI Codex** — assistance au développement par IA

Voir [`CONTRIBUTORS.md`](CONTRIBUTORS.md).

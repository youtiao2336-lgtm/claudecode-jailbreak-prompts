# Gestionnaire de prompts Claude Code

[简体中文](README.md) · [English](README.en.md) · [日本語](README.ja.md) · **Français** · [Русский](README.ru.md)

![Icône ccprompt](assets/app-preview.png)

Gestionnaire de prompts Claude Code est un outil local Windows de gestion de prompts. Son interface graphique, son interface en ligne de commande et ses scripts PowerShell utilisent tous les fichiers Markdown du dossier `prompts/`, avec un même système de sauvegarde, d’écriture par lots et de restauration.

![Interface ccprompt](gui-screenshot.png)

## Fonctionnalités

- Sélectionner plusieurs prompts et les fusionner dans le fichier `CLAUDE.md` cible, dans l’ordre choisi.
- Utiliser une cible utilisateur, une cible de projet ou un chemin personnalisé.
- Créer automatiquement une sauvegarde avant l’écriture et restaurer le fichier en un clic.
- Détecter les dossiers de configuration et les emplacements des applications locales.
- Créer, modifier et supprimer des prompts depuis l’interface graphique.
- Basculer l’application entre le chinois simplifié, l’anglais, le japonais, le français et le russe, avec mémorisation du choix.
- Utiliser les noms de fichiers, titres et contenus intégrés correspondant à la langue choisie.
- Adapter automatiquement les listes, l’éditeur, le journal et les commandes au redimensionnement des fenêtres.
- Partager le même dossier de prompts entre l’interface graphique, la CLI et PowerShell.

## Démarrage rapide

1. Téléchargez le paquet Windows, puis conservez `ccprompt-gui.exe`, `prompts/` et `inject/` dans le même dossier.
2. Lancez `ccprompt-gui.exe`.
3. Choisissez la langue en bas de la fenêtre principale, sélectionnez les prompts et la cible, puis cliquez sur « Activer ».
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

## Format des prompts

Chaque prompt est un fichier `.md` indépendant dans `prompts/`. Le nom du fichier sert d’identifiant et le premier titre de niveau 1 sert de nom d’affichage.

Les prompts chinois se trouvent dans `prompts/` ; les versions anglaise, japonaise, française et russe se trouvent dans `prompts/en/`, `prompts/ja/`, `prompts/fr/` et `prompts/ru/`. L’application affiche et édite automatiquement les sept fichiers localisés correspondant à la langue sélectionnée.

## Sources des prompts intégrés

Les sept modules intégrés réorganisent des structures de prompts, méthodes d’injection et mécanismes de persistance publiés sur GitHub. Les principaux auteurs d’origine sont l’**équipe Piebald / Piebald LLC** pour [tweakcc](https://github.com/Piebald-AI/tweakcc), **0xSufi** pour [projet de prompts Fable](https://github.com/0xSufi/fable-jailbreak), **momori777** pour [Artemis](https://github.com/momori777/Artemis) et **twaai** pour le prompt original ensuite publié sur GitHub par deeropa.

Consultez [`SOURCES.md`](SOURCES.md) pour les sources et attributions complètes.

## Contributeurs et remerciements

- **youtiao2336-lgtm** — auteur et mainteneur du projet
- **OpenAI Codex** — assistance au développement par IA

Voir [`CONTRIBUTORS.md`](CONTRIBUTORS.md).

# plummet_game-Saad


# Plummet Game

## Description du jeu

**Plummet Game** Le joueur dois se rendre a la fin

## Hiérarchie des classes

### GameManager
Le `GameManager` est responsable de la gestion générale du jeu. Il supervise le jeu, gère les événements de fin de jeu et met à jour le score. Il contient des références à d'autres systèmes comme le **Player** et le **ScoreManager**.

### Player
La classe `Player` gère le personnage du joueur. Elle s'occupe des déplacements, des collisions et de l'énergie. Elle interagit avec l'environnement et gère également le mode automatique (AI) ou manuel du joueur.

- **Classes associées** :
  - `MovementController` : Gère les entrées du joueur et le déplacement.
  - `CollisionHandler` : Gère les collisions du joueur avec les murs.
  - `EnergyManager` : Gère l'énergie du joueur et l'effet des collisions sur cette énergie.

### ScoreManager
Le `ScoreManager` est responsable de la gestion du score du joueur. Il suit le nombre de collisions, l'énergie restante et les murs détruits. Il met à jour le score en temps réel et l'affiche à l'aide de l'interface utilisateur.

- **Classes associées** :
  - `UIManager` : Gère l'affichage du score avec les éléments Unity UI (par exemple `Text`).

### Wall
La classe `Wall` représente les obstacles que le joueur doit franchir. Elle gère les comportements des murs, y compris leur destruction après une collision avec le joueur.

- **Classes associées** :
  - `WallDestroyer` : Gère la destruction des murs après les collisions.

### AIManager
Le `AIManager` gère le mode automatique du joueur. Lorsqu'il est activé, il permet au personnage de se déplacer automatiquement en utilisant un algorithme de recherche de chemin comme Djikstra. Si le chemin est bloqué, le personnage peut reculer et essayer un autre chemin.

- **Classes associées** :
  - `Pathfinding` : Implémente l'algorithme de Djikstra pour rechercher un chemin optimal.
  - `MovementController` : Utilise les résultats du `AIManager` pour déplacer le joueur automatiquement.

### GameEventManager
Le `GameEventManager` gère les événements du jeu, comme la fin du jeu ou le redémarrage. Il déclenche des événements comme `GameOverEvent` lorsque le jeu est terminé (quand l'énergie du joueur est épuisée ou qu'il atteint la ligne d'arrivée).

- **Classes associées** :
  - `GameOverEvent` : Gère l'événement de fin du jeu et déclenche l'action de redémarrage.

## Fonctionnalités

- **Gestion du score** : Le score est calculé en fonction de l'énergie restante, du nombre de collisions et du nombre de murs détruits.
- **Mode automatique AI** : Le joueur peut activer le mode AI pour que le personnage se déplace automatiquement.
- **Système d'événements** : Le jeu détecte la fin du jeu et redémarre en fonction de l'état de l'énergie du joueur et de l'atteinte de la ligne d'arrivée.
- **Sauvegarde de la progression** : Les scores sont enregistrés dans une base de données MongoDB pour conserver la progression du joueur.




----------------------------------------------------------

## Patrons de Conception

### 1. **Facade (GameManager)**
Le patron **Facade** est utilisé dans la classe `GameManager` pour fournir une interface simplifiée pour les interactions avec le jeu. Vous utilisez `GameManager` pour démarrer, arrêter ou redémarrer le jeu, en masquant la complexité sous-jacente du système de jeu.

### 2. **Singleton (ScoreManager)**
Le patron **Singleton** est utilisé dans la classe `ScoreManager` pour garantir qu'il n'y a qu'une seule instance de `ScoreManager` tout au long du jeu. Cela permet de garantir une gestion cohérente du score global à travers l'ensemble du jeu.

### 3. **Strategy (Player, AIManager)**
Le patron **Strategy** est appliqué dans les classes `Player` et `AIManager` pour permettre de changer dynamiquement la manière dont le joueur se déplace. Selon le mode de jeu, le joueur peut soit se déplacer manuellement, soit être contrôlé par l'IA. Le comportement de déplacement peut donc être modifié à la volée.

### 4. **State (Player, AIManager)**
Le patron **State** est utilisé pour gérer les différents états dans lesquels un joueur ou un système peut se trouver. Par exemple, un joueur ou une IA peut être dans un état de "mouvement" ou dans un état "bloqué", et ce patron permet de gérer de manière fluide ces transitions d'état.

### 5. **Observer (Wall, GameEventManager)**
Le patron **Observer** est utilisé dans les classes `Wall` et `GameEventManager` pour permettre aux objets du jeu de réagir à certains événements. Par exemple, les murs réagissent aux collisions avec le joueur et se détruisent en conséquence. De plus, `GameEventManager` alerte le jeu lorsqu'un événement important (comme la fin du jeu) se produit, permettant à d'autres systèmes de réagir en conséquence.

### 6. **Composite (Score)**
Le patron **Composite** est utilisé dans la classe `Score` pour structurer le score du joueur. Le score est une combinaison de plusieurs facteurs, comme l'énergie restante, le nombre de collisions, et les murs détruits. Ce patron permet de gérer facilement ces éléments complexes comme un tout cohérent.









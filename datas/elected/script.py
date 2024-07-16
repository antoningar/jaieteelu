import csv

chemin_fichier = 'liste_deputes_libre_office.csv'
chemin_sortie = 'elus.txt'

with open(chemin_fichier, newline='', encoding='utf-8') as fichier:
    lecteur = csv.DictReader(fichier)

    with open(chemin_sortie, 'w', encoding='utf-8') as fichier_sortie:
        for ligne in lecteur:
            nom_prenom = f"{ligne['Prénom']} {ligne['Nom']}\n"
            fichier_sortie.write(nom_prenom)
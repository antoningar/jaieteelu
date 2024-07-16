import json

# Charger les données JSON
chemin_fichier_json = 'facts/parsed_candidats.json'
with open(chemin_fichier_json, 'r', encoding='utf-8') as fichier:
    donnees_json = json.load(fichier)

# Lire les noms à partir du fichier texte
chemin_fichier_noms = 'elected/elus.txt'
noms_a_trouver = set()
with open(chemin_fichier_noms, 'r', encoding='utf-8') as fichier:
    for ligne in fichier:
        # Nettoyer et préparer les noms pour la correspondance
        nom_purifie = ligne.strip().replace("/", " ")
        noms_a_trouver.add(nom_purifie)

# Liste pour stocker les résultats correspondants
resultats_correspondants = []

# Vérifier les correspondances
for element in donnees_json:
    # Nettoyer le nom dans le JSON pour la correspondance
    nom_json = element["nom"].lower() 
    print(nom_json)
    if any(nom.lower() in nom_json for nom in noms_a_trouver):
        resultats_correspondants.append(element)

# Écrire les résultats dans un nouveau fichier JSON
chemin_nouveau_json = 'results.json'
with open(chemin_nouveau_json, 'w', encoding='utf-8') as nouveau_fichier:
    json.dump(resultats_correspondants, nouveau_fichier, ensure_ascii=False, indent=4)

print(len(resultats_correspondants))
import requests

from .utils import *
from .constants import *
from time import sleep, time

params = dict(election_type = None, state_id = None)

def retrieve_data():
    from bs4 import BeautifulSoup

    path_check(SAMPLES_DIR)

    session = requests.Session()

    start = time()

    # elections = [
    #     [SENATE, HEADERS_SEN, "districts"],
    #     [REPS, HEADERS_REP, "federal_constituencies"],
    # ]

    # params = {
    #     'election_type': None
    # }

    # for election, header, file in elections:
    #     params['election_type'] = election
    #     assm_req = session.get(ASSM_URL, headers=header, params=params)
    #     nass_table(assm_req.json(), SAMPLES_DIR, file)

    #     sleep(1)

    collections_req = session.get(PVC_URL)
    collections_sp = BeautifulSoup(collections_req.content, 'html.parser')

    stats = collections_sp.find_all('div', class_ = 'state-box')

    lgas = []
    wards = []
    polling_units = []

    for i, stat in enumerate(stats, 1):
        name = state_case(stat.text)

        link = stat.find('a')['href']
        tbl_req = session.get(link)
        lgas.append(extract(tbl_req.content, 0, name))
        wards.append(extract(tbl_req.content, 1, name))
        polling_units.append(extract(tbl_req.content, 2, name))
        
        sleep(1)

    save_file(pd.concat(lgas, ignore_index=True), 'lgas')
    save_file(pd.concat(wards, ignore_index=True), 'wards')
    save_file(pd.concat(polling_units, ignore_index=True), 'polling_units')

    stop = time()

    calc_time(start, stop)


def clean_data():

    def is_kano(row):
        row.Ward = row.Ward.removeprefix('\'')
        if 'GWANJE' in row.Ward:
            row.Ward = row.Ward.replace('.', '')
        return row
    
    def replace_hyphen(match):
        return match.group(1) + ' ' + match.group(2)

    path_check(SAMPLES_DIR)
    if not any(os.listdir(SAMPLES_DIR)):
        logger.info('No files in directory: %s' %(SAMPLES_DIR))
        retrieve_data()
    
    districts = pd.read_csv(os.path.join(SAMPLES_DIR, 'districts.csv'))
    fed_consts = pd.read_csv(os.path.join(SAMPLES_DIR, 'federal_constituencies.csv'))
    lgas = pd.read_csv(os.path.join(SAMPLES_DIR, 'lgas.csv'))
    pu = pd.read_csv(os.path.join(SAMPLES_DIR, 'polling_units.csv'))
    wards = pd.read_csv(os.path.join(SAMPLES_DIR, 'wards.csv'))

    # pattern = re.compile(r'(\S+)\s*(?:-?\s*(SOUTH|WEST|NORTH|EAST)\b|\b(SOUTH|WEST|NORTH|EAST)\s*-?)')
    # lgas['Name'] = lgas['Name'].apply(lambda x: pattern.sub(lambda m: m.group(1).replace('-','') + ' ' + (m.group(2) or m.group(3)) if m else x, x))
    # lgas['Name'] = lgas['Name'].apply(lambda x: re.sub(pattern, replace_hyphen, x))

    lgas.replace(
        {'Name':
            {'FUFORE':'FURORE','MAYO-BELWA':'MAYO BELWA', 'IHALA':'IHIALA', 'GANJUWA':'GUNJUWA', 'ITAS/GADAU':'ITAS-GADAU',
            'KOLOKUMA/OPOKUMA':'KOLOKUNA/OPOKUMA', 'OGBADIBO':'OBADIGBO', 'ASKIRA / UBA':'ASKIRA-UBA', 'GUZAMALA':'GUZAMALI', 
            'KWAYA / KUSAR':'KWAYA-KUSAR', 'KALA BALGE':'KALA-BALGE','MAIDUGURI M. C.':'MAIDUGURI (METROPOLITAN)', 
            'NDOKWA EAST':'NKOKWA EAST', 'IKPOBA/OKHA':'IKPOBA-OKHA', 'AKOKO EDO':'AKOKO-EDO', 'ESAN WEST':'ESAN SOUTH', 
            'UHUNMWODE':'UHUNMWONDE', 'ILEJEMEJE':'ILEJEME', 'ISE / ORUN':'ORUN/ISE', 'IGBO ETITI':'IGBO-ETITI',
            'IGBO EZE NORTH':'IGBO-EZE NORTH', 'IGBO EZE SOUTH':'IGBO-EZE SOUTH', 'OJI-RIVER':'OJI RIVER',
            'NASARAWA':'NASSARAWA', 'NASARAWA EGGON':'NASSARAWA EGGON', 'BOSSO':'BOOSO', 'EDATTI':'EDATI',
            'EMOHUA':'UMOHUA', 'OBIO/AKPOR':'OBIO AKPOR', 'S/BIRNI':'SABON BIRNI', 'KARIM-LAMIDO':'KARIM LAMIDO', 
            'POTISKUM':'POTISKM', 'KARASAWA':'KARASUWA', 'KAURA NAMODA':'KAURA-NAMODA'
        }
    }, inplace=True)

    # wards['Name'] = wards['Name'].apply(lambda x: x.removesuffix(' Total'))
    # pu = pu.apply(is_kano, axis=1)

    # name = 'LGA'
    # if name not in wards.columns:
    #     wards.insert(1, name, None)
    # w_c = pu['Ward'].value_counts()
    # for ward in wards['Name'].values:
    #     w_lga = pu.loc[pu.Ward == ward].iat[0, 1]
    #     wards.loc[wards.loc[wards['Name'] == ward].index, ['NoOfPollingUnits', 'LGA']] = [w_c[ward], w_lga]
    #     logger.info('Found LGA: %s for: %s', w_lga, ward)
    
    # l_c = wards.LGA.value_counts()
    # p_c = pu['LGA'].value_counts()
    count = 0
    for lga in lgas.itertuples():
        name = lga.Name
        fed = fed_consts.loc[fed_consts['Name'].str.contains(name, regex=False)]
        if fed.empty:
            count += 1
            logger.info('Missing Constituency for: %s - %s', name, lga.State)
            continue
        
    # #    lgas.loc[lgas.loc[lgas['Name'] == lga].index, ['NoOfPollingUnits', 'FederalConstituency']] = [p_c[lga], fed.Name]
    #    logger.info('Found Constituency: %s for: %s', fed['Name'].to_list(), lga)

    print(count)




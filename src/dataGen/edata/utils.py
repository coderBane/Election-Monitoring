import os
import re
import logging
import pandas as pd
from . import constants as ct

logging.basicConfig(format='%(asctime)s - %(levelname)s : %(message)s',level= logging.DEBUG)
logger = logging.getLogger()

def path_check(path:str):
    if os.path.exists(path):
        logger.info('Located directory %s', path)
        return
    os.makedirs(path, exist_ok=True) 
    logger.info('Created directory %s', path)


def state_case(name:str):
    n = name.strip()
    return n.upper() if n.casefold().startswith('f') else n.title().replace(' ', '')


def nass_table(result:dict, path:str, file:str, is_fed = True):
    cp = os.path.join(path, file)
    schema = {'State':str, 'Name':str, '_id':str, 'IsFed':bool}
    df = pd.DataFrame(columns=schema.keys()).astype(schema)

    data = result['data']
    if not any(data):
        logger.error('No data found for %s', file)
        return
    
    for i, x in enumerate(data, start=1):
        _id = x['_id']
        name = re.sub(r'\s*/\s*', '/', x['domain']['name'].strip().upper())
        state = state_case(x['state']['name'])
        df = pd.concat([df, pd.DataFrame({"State": state, "Name": name, "_id": _id, "IsFed": is_fed}, index=[i])], ignore_index=True)

    df.drop_duplicates(subset=['Name'], inplace=True)
    save_file(df, file)

    # ids = [x['_id'].strip() for x in result['data']]
    # consts = [x['domain']['name'].strip().upper() for x in result['data']]
    # state = result['data'][0]['state']['name'].title().strip().replace(' ', '')
    # df['Name'], df['_id'] = consts, ids
    # df['State'], df['IsFed'] = state, is_fed
    # df.drop_duplicates(subset=['Name'], inplace=True)
    # df.to_csv(cp)

    # logger.info('Created file %s : %s', name, path)


def extract(content:bytes, index:int, state:str, col_names = 3):
    PATTERN = re.compile(r'(\S+)\s*(?:-?\s*(SOUTH|WEST|NORTH|EAST)\b|\b(SOUTH|WEST|NORTH|EAST)\s*-?)')
    FUNC = lambda x: PATTERN.sub(lambda m: m.group(1).replace('-','') + ' ' + (m.group(2) or m.group(3)) if m else x, x)
    def table_name(): 
        match index:
            case 1: return 'wards'
            case 2: return 'polling units'
            case _: return 'lgas'

    try:
        df = pd.read_html(content, header=col_names)[index]
        if df.empty:
            logger.warning('Could not extract content')
            return
    except:
        logger.error('Could not extract content')
        return
    
    logger.info('Extracted table %s for: %s', table_name(), state)
    df = df.iloc[:-1, 2:-2]
    if index == 2:
        df.columns = ['State', 'LGA', 'Ward', 'Name', 'Delimitation', 'RegisteredVoters', 'EligibleVoters']
        df['State'] = df['State'].apply(state_case)
        df['LGA'] = df['LGA'].str.replace(' - ', '-').apply(FUNC)
        df['Ward'].str.removeprefix('\'')
    else:
        df.insert(0, 'State', state)
        df.columns = ['State', 'Name', 'RegisteredVoters', 'EligibleVoters']
        if index == 0:
            df['Name'] = df['Name'].str.replace(' - ', '-').apply(FUNC)

    return df


def save_file(df:pd.DataFrame, name:str, ext='csv'):
    if ext not in {'csv', 'json'}:
        logger.warning('Cannot save file with extension %s' %(ext))
        return
    
    save = getattr(df,f'to_{ext}')
    
    cp = os.path.join(ct.SAMPLES_DIR, name + '.' + ext)
    ex = os.path.isfile(cp)
    if ext == 'csv': save(cp, index=False)
    if ext == 'json': save(cp, orient='records')

    if ex:
        logger.info('OverWritten file \'%s\' : %s' %(name, ct.SAMPLES_DIR))
    else:
        logger.info ('Created file \'%s\' : %s' %(name, ct.SAMPLES_DIR))


def calc_time(start:float, stop:float):
    elapsed_time = stop - start
    hrs, rem = divmod(elapsed_time, 3600)
    mins, sec = divmod(rem, 60)
    logging.debug('Executed program in %s', f'HH:{hrs:02.0f} MM:{mins:02.0f} SS:{sec:.2f}')

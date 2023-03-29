import os

# from time import sleep, time
# from requests import Session
# from bs4 import BeautifulSoup
import edata.data as data
# from constants import *


# def retrieve_tables():

#     collections_req = session.get(pvc_url)
#     collections_sp = BeautifulSoup(collections_req.content, 'html.parser')

#     stats = collections_sp.find_all('div', class_ = 'state-box')

#     path_check(SAMPLES_DIR)

#     lgas = []
#     wards = []
#     polling_units = []

#     for i, stat in enumerate(stats, start=1):
#         name = str(stat.text).strip()
#         name = name if name.startswith('F') else name.title().replace(' ', '')

#     #     link = stat.find('a')['href']
#     #     tbl_req = session.get(link)

#     #     lgas, rv, ev = toCsv(tbl_req.content, 0, state_dir, 'lga.csv')
#     #     ra, _, _ = toCsv(tbl_req.content, 1, state_dir, 'ward.csv')
#     #     _, _, _ = toCsv(tbl_req.content, 2, state_dir, 'pu.csv')

#     #     sleep(1)

#     stop = time()
#     calc_time(start, stop)


if __name__ == '__main__':
    data.retrieve_data()
    # data.clean_data()
